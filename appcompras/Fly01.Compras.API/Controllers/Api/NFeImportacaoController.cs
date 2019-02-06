using Fly01.Compras.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("nfeimportacao")]
    public class NFeImportacaoController : ApiPlataformaController<NFeImportacao, NFeImportacaoBL>
    {
        private readonly string routePrefixNamePessoa = @"Pessoa";
        private readonly string routePrefixNameProduto = @"Produto";
        private readonly string routePrefixNameMovimentoPedidoCompra = @"MovimentoPedidoCompra";
        private readonly string routePrefixNameContaPagar = @"ContaPagar";

        public NFeImportacaoController()
        {
            //TODO: diego MustExecuteAfterSave = true;
        }


        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<NFeImportacao> model)
        {
            if (model == null || key == default(Guid) || key == null)
                return BadRequest(ModelState);

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                throw new BusinessException("Registro não encontrado ou já excluído");

            if (entity.RegistroFixo)
                throw new BusinessException("Registro não pode ser editado (RegistroFixo)");

            ModelState.Clear();
            model.Patch(entity);
            Update(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            try
            {
                await UnitSave();
                await FinalizarESalvarDados(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(key))
                    return NotFound();
                else
                    throw;
            }

            return Ok();
        }

        private async Task FinalizarESalvarDados(NFeImportacao entity)
        {
            var NFe = UnitOfWork.NFeImportacaoBL.DeserializeXMlToNFe(entity.Xml);           
            try
            {
                #region Fornecedor

                var ibge = NFe.InfoNFe.Emitente?.Endereco?.CodigoMunicipio;
                var cidade = UnitOfWork.CidadeBL.All.FirstOrDefault(x => x.CodigoIbge == ibge);

                if (entity.NovoFornecedor)
                {
                    entity.FornecedorId = Guid.NewGuid();
                    var fornecedor = new Pessoa()
                    {
                        Id = entity.FornecedorId.Value,
                        Nome = NFe.InfoNFe.Emitente?.Nome,
                        TipoDocumento = NFe.InfoNFe.Emitente?.Cnpj.Length == 14 ? "J" : "F",
                        CPFCNPJ = NFe.InfoNFe.Emitente?.Cnpj != null ? NFe.InfoNFe.Emitente?.Cnpj : NFe.InfoNFe.Emitente?.Cpf != null ? NFe.InfoNFe.Emitente?.Cpf : null,
                        NomeComercial = NFe.InfoNFe.Emitente?.NomeFantasia,
                        Endereco = NFe.InfoNFe.Emitente?.Endereco?.Logradouro,
                        Numero = NFe.InfoNFe.Emitente?.Endereco?.Numero,
                        Complemento = NFe.InfoNFe.Emitente?.Endereco?.Numero,
                        Bairro = NFe.InfoNFe.Emitente?.Endereco?.Bairro,
                        CidadeId = cidade?.Id,
                        EstadoId = cidade?.EstadoId,
                        CEP = NFe.InfoNFe.Emitente?.Endereco?.Cep,
                        InscricaoEstadual = NFe.InfoNFe.Emitente?.InscricaoEstadual,
                        Fornecedor = true
                    };
                    UnitOfWork.PessoaBL.Insert(fornecedor);
                    await UnitSave();
                    Producer<Pessoa>.Send(routePrefixNamePessoa, AppUser, PlataformaUrl, fornecedor, RabbitConfig.EnHttpVerb.POST);

                }
                else if (entity.AtualizaDadosFornecedor)
                {
                    entity.NovoFornecedor = false;
                    var fornecedor = UnitOfWork.PessoaBL.Find(entity.FornecedorId);
                    if (fornecedor != null)
                    {
                        fornecedor.Nome = NFe.InfoNFe.Emitente?.Nome;
                        fornecedor.CPFCNPJ = NFe.InfoNFe.Emitente?.Cnpj != null ? NFe.InfoNFe.Emitente?.Cnpj : NFe.InfoNFe.Emitente?.Cpf != null ? NFe.InfoNFe.Emitente?.Cpf : null;
                        fornecedor.NomeComercial = NFe.InfoNFe.Emitente?.NomeFantasia;
                        fornecedor.Endereco = NFe.InfoNFe.Emitente?.Endereco?.Logradouro;
                        fornecedor.Numero = NFe.InfoNFe.Emitente?.Endereco?.Numero;
                        fornecedor.Complemento = NFe.InfoNFe.Emitente?.Endereco?.Numero;
                        fornecedor.Bairro = NFe.InfoNFe.Emitente?.Endereco?.Bairro;
                        fornecedor.CidadeId = cidade?.Id;
                        fornecedor.EstadoId = cidade?.EstadoId;
                        fornecedor.CEP = NFe.InfoNFe.Emitente?.Endereco?.Cep;
                        fornecedor.InscricaoEstadual = NFe.InfoNFe.Emitente?.InscricaoEstadual;
                        UnitOfWork.PessoaBL.Update(fornecedor);
                        await UnitSave();
                        Producer<Pessoa>.Send(routePrefixNamePessoa, AppUser, PlataformaUrl, fornecedor, RabbitConfig.EnHttpVerb.PUT);
                    }
                    else
                    {
                        throw new BusinessException("Fornecedor não localizado para atualização dos dados.");
                    }
                }
                #endregion

                #region Transportadora

                var hasTagTransportadora = (NFe != null && NFe.InfoNFe != null && NFe.InfoNFe.Transporte != null && NFe.InfoNFe.Transporte.Transportadora != null && NFe.InfoNFe.Transporte.Transportadora?.RazaoSocial != null);
                if (entity.NovaTransportadora)
                {
                    if (hasTagTransportadora && entity.TipoFrete != TipoFrete.SemFrete)
                    {
                        entity.TransportadoraId = Guid.NewGuid();
                        var transportadora = new Pessoa()
                        {
                            Id = entity.TransportadoraId.Value,
                            TipoDocumento = NFe.InfoNFe.Emitente?.Cnpj.Length == 14 ? "J" : "F",
                            CPFCNPJ = NFe.InfoNFe.Emitente?.Cnpj != null ? NFe.InfoNFe.Emitente?.Cnpj : NFe.InfoNFe.Emitente?.Cpf != null ? NFe.InfoNFe.Emitente?.Cpf : null,
                            Nome = NFe.InfoNFe.Emitente?.Nome,
                            InscricaoEstadual = NFe.InfoNFe.Emitente?.InscricaoEstadual,
                            Endereco = NFe.InfoNFe.Emitente?.Endereco?.Logradouro,
                            CidadeId = cidade?.Id,
                            EstadoId = cidade?.EstadoId,
                            Transportadora = true
                        };
                        UnitOfWork.PessoaBL.Insert(transportadora);
                        await UnitSave();
                        Producer<Pessoa>.Send(routePrefixNamePessoa, AppUser, PlataformaUrl, transportadora, RabbitConfig.EnHttpVerb.POST);
                    }
                }
                else if (entity.AtualizaDadosTransportadora)
                {
                    if (hasTagTransportadora)
                    {
                        entity.NovaTransportadora = false;
                        var transportadora = UnitOfWork.PessoaBL.Find(entity.TransportadoraId);
                        if (transportadora != null)
                        {
                            transportadora.Id = entity.TransportadoraId.Value;
                            transportadora.CPFCNPJ = NFe.InfoNFe.Emitente?.Cnpj != null ? NFe.InfoNFe.Emitente?.Cnpj : NFe.InfoNFe.Emitente?.Cpf != null ? NFe.InfoNFe.Emitente?.Cpf : null;
                            transportadora.Nome = NFe.InfoNFe.Emitente?.Nome;
                            transportadora.InscricaoEstadual = NFe.InfoNFe.Emitente?.InscricaoEstadual;
                            transportadora.Endereco = NFe.InfoNFe.Emitente?.Endereco?.Logradouro;
                            transportadora.CidadeId = cidade?.Id;
                            transportadora.EstadoId = cidade?.EstadoId;
                            UnitOfWork.PessoaBL.Update(transportadora);
                            await UnitSave();
                            Producer<Pessoa>.Send(routePrefixNamePessoa, AppUser, PlataformaUrl, transportadora, RabbitConfig.EnHttpVerb.PUT);
                        }
                    }
                }
                #endregion

                #region Produto
                foreach (var item in UnitOfWork.NFeImportacaoProdutoBL.All.Where(x => x.NFeImportacaoId == entity.Id))
                {
                    var nfeproduto = NFe.InfoNFe.Detalhes.Where(x => x.Produto.GTIN == item.CodigoBarras).Select(x => x.Produto).FirstOrDefault();

                    if (item.NovoProduto)
                    {
                        item.ProdutoId = Guid.NewGuid();
                        var novoproduto = new Produto()
                        {
                            Id = item.ProdutoId.Value,
                            Descricao = item.Descricao,
                            CodigoBarras = item.CodigoBarras,
                            UnidadeMedidaId = item.UnidadeMedidaId,
                            ValorCusto = FatorConversaoValor(item.FatorConversao, item.Valor, item.TipoFatorConversao),
                            ValorVenda = item.AtualizaValorVenda ? item.ValorVenda : 0,
                            NcmId = UnitOfWork.NCMBL.All.FirstOrDefault(x => x.Codigo == nfeproduto.NCM)?.Id,
                            CestId = UnitOfWork.CestBL.All.FirstOrDefault(x => x.Codigo == nfeproduto.CEST)?.Id,
                            SaldoProduto = item.MovimentaEstoque ? FatorConversaoQuantidade(item.FatorConversao, item.Quantidade, item.TipoFatorConversao) : 0
                        };

                        UnitOfWork.ProdutoBL.Insert(novoproduto);
                        await UnitSave();
                        Producer<Produto>.Send(routePrefixNameProduto, AppUser, PlataformaUrl, novoproduto, RabbitConfig.EnHttpVerb.POST);
                        UnitOfWork.NFeImportacaoProdutoBL.Update(item);
                        await UnitSave();
                    }
                    else if (item.AtualizaDadosProduto || item.AtualizaValorVenda)
                    {
                        var produto = UnitOfWork.ProdutoBL.Find(item.ProdutoId);
                        if (produto != null)
                        {
                            if (item.AtualizaDadosProduto)
                            {

                                produto.Descricao = item.Descricao;
                                produto.CodigoBarras = item.CodigoBarras;
                                produto.ValorCusto = FatorConversaoValor(item.FatorConversao, item.Valor, item.TipoFatorConversao);
                                produto.NcmId = UnitOfWork.NCMBL.All.FirstOrDefault(x => x.Codigo == nfeproduto.NCM)?.Id;
                                produto.CestId = UnitOfWork.CestBL.All.FirstOrDefault(x => x.Codigo == nfeproduto.CEST)?.Id;
                            }
                            else
                            {
                                produto.ValorVenda = item.AtualizaValorVenda ? item.ValorVenda : produto.ValorVenda;
                            }

                            UnitOfWork.ProdutoBL.Update(produto);
                            await UnitSave();
                            Producer<Produto>.Send(routePrefixNameProduto, AppUser, PlataformaUrl, produto, RabbitConfig.EnHttpVerb.PUT);
                        }
                    }
                }
                #endregion

                #region MovimentaEstoque

                foreach (var item in UnitOfWork.NFeImportacaoProdutoBL.All.Where(x => x.NFeImportacaoId == entity.Id && x.MovimentaEstoque && !x.NovoProduto))

                {
                    var movimentos = (from nfeimportacaoproduto in UnitOfWork.NFeImportacaoProdutoBL.All.Where(x => x.NFeImportacaoId == entity.Id && x.MovimentaEstoque && !x.NovoProduto)
                                      group nfeimportacaoproduto by nfeimportacaoproduto.ProdutoId into groupResult
                                      select new
                                      {
                                          ProdutoId = groupResult.Key,
                                          Total = groupResult.Sum(f => FatorConversaoQuantidade(f.FatorConversao, f.Quantidade, f.TipoFatorConversao))
                                      })
                        .Select(x => new MovimentoPedidoCompra
                        {
                            Quantidade = (x.Total),
                            ProdutoId = x.ProdutoId.Value,
                            UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao,
                            TipoCompra = entity.Tipo,
                            PlataformaId = PlataformaUrl,
                            Serie = entity.Serie,
                            Numero = entity.Numero,
                            IsNFeImportacao = true
                        }).ToList();

                    foreach (var movimento in movimentos)
                        Producer<MovimentoPedidoCompra>.Send(routePrefixNameMovimentoPedidoCompra, AppUser, PlataformaUrl, movimento, RabbitConfig.EnHttpVerb.POST);
                }

                #endregion

                #region Pedido
                if (entity.NovoPedido)
                {
                    entity.PedidoId = Guid.NewGuid();
                    var pedido = new Pedido()
                    {
                        Id = entity.PedidoId.Value,
                        TipoOrdemCompra = TipoOrdemCompra.Pedido,
                        FornecedorId = entity.FornecedorId.Value,
                        TransportadoraId = entity.TransportadoraId,
                        NFeImportacaoId = entity.Id,
                        TipoCompra = entity.Tipo,
                        Status = StatusOrdemCompra.Aberto,
                        Data = DateTime.Now,//NFe.InfoNFe.Identificador.Emissao,
                        Total = entity.ValorTotal,
                        TipoFrete = NFe.InfoNFe.Transporte.ModalidadeFrete,
                        Marca = NFe.InfoNFe.Transporte?.Volume?.Marca,
                        PlacaVeiculo = NFe.InfoNFe?.Transporte?.Veiculo?.Placa,
                        TipoEspecie = NFe.InfoNFe.Transporte?.Volume?.Especie,
                        ValorFrete = entity.ValorFrete,
                        PesoBruto = NFe.InfoNFe.Transporte?.Volume?.PesoBruto,
                        PesoLiquido = NFe.InfoNFe.Transporte?.Volume?.PesoLiquido,
                        QuantidadeVolumes = NFe.InfoNFe.Transporte?.Volume?.Quantidade,
                        FormaPagamentoId = entity.FormaPagamentoId,
                        CategoriaId = entity.CategoriaId,
                        CondicaoParcelamentoId = entity.CondicaoParcelamentoId,
                        DataVencimento = entity.DataVencimento,
                        Observacao = string.Format("Pedido gerado através da importação do XML da nota {0} - {1}.", entity.Serie, entity.Numero)
                    };

                    UnitOfWork.PedidoBL.Insert(pedido);
                    await UnitSave();

                    foreach (var item in UnitOfWork.NFeImportacaoProdutoBL.All.Where(x => x.NFeImportacaoId == entity.Id))
                    {
                        item.PedidoItemId = Guid.NewGuid();
                        UnitOfWork.PedidoItemBL.Insert(new PedidoItem()
                        {
                            Id = item.PedidoItemId.Value,
                            PedidoId = entity.PedidoId.Value,
                            NFeImportacaoProdutoId = item.Id,
                            ProdutoId = item.ProdutoId.Value,
                            Quantidade = item.Quantidade,
                            Valor = item.Valor
                        });
                        UnitOfWork.NFeImportacaoProdutoBL.Update(item);
                    }
                    pedido.Status = StatusOrdemCompra.Finalizado;
                    UnitOfWork.PedidoBL.Update(pedido);
                    await UnitSave();
                }
                #endregion

                #region Financeiro
                if (entity.GeraFinanceiro)
                {
                    if (entity.GeraContasXml && UnitOfWork.NFeImportacaoCobrancaBL.All.Any(x => x.NFeImportacaoId == entity.Id))
                    {
                        var condicacaoparcelamento = UnitOfWork.CondicaoParcelamentoBL.All.FirstOrDefault(x => x.QtdParcelas == 0 || x.CondicoesParcelamento == "0").Id;
                        if (condicacaoparcelamento == null)
                        {
                            condicacaoparcelamento = Guid.NewGuid();
                            var novacondicao = new CondicaoParcelamento()
                            {
                                Id = condicacaoparcelamento,
                                QtdParcelas = 1,
                                Descricao = "Avista"
                            };
                            UnitOfWork.CondicaoParcelamentoBL.Update(novacondicao);
                            Producer<CondicaoParcelamento>.Send(routePrefixNameContaPagar, AppUser, PlataformaUrl, novacondicao, RabbitConfig.EnHttpVerb.POST);
                        }
                        foreach (var item in UnitOfWork.NFeImportacaoCobrancaBL.All.Where(x => x.NFeImportacaoId == entity.Id))
                        {

                            var contaPagar = new ContaPagar()
                            {
                                ValorPrevisto = item.Valor,
                                CategoriaId = entity.CategoriaId.Value,
                                CondicaoParcelamentoId = condicacaoparcelamento,
                                PessoaId = entity.FornecedorId.Value,
                                DataEmissao = DateTime.Now,
                                DataVencimento = item.DataVencimento,
                                Observacao = string.Format("Conta a Pagar gerada através da importação do XML da nota {0} - {1}, aplicativo Bemacash Compras", entity.Serie, entity.Numero),
                                Descricao = string.Format("Importação do XML, nota {0} - {1}", entity.Serie, entity.Numero),
                                FormaPagamentoId = entity.FormaPagamentoId.Value,
                            };
                            Producer<ContaPagar>.Send(routePrefixNameContaPagar, AppUser, PlataformaUrl, contaPagar, RabbitConfig.EnHttpVerb.POST);
                        }
                    }
                    else
                    {
                        var contaPagar = new ContaPagar()
                        {
                            ValorPrevisto = entity.ValorTotal,
                            CategoriaId = entity.CategoriaId.Value,
                            CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                            PessoaId = entity.FornecedorId.Value,
                            DataEmissao = DateTime.Now,
                            DataVencimento = entity.DataVencimento,
                            Descricao = string.Format("Importação do XML, nota {0} - {1}", entity.Serie, entity.Numero),
                            Observacao = string.Format("Conta a Pagar gerada através da importação do XML da nota {0} - {1}, aplicativo Bemacash Compras", entity.Serie, entity.Numero),
                            FormaPagamentoId = entity.FormaPagamentoId.Value,
                        };
                        Producer<ContaPagar>.Send(routePrefixNameContaPagar, AppUser, PlataformaUrl, contaPagar, RabbitConfig.EnHttpVerb.POST);
                    }
                }
                #endregion
                UnitOfWork.NFeImportacaoBL.Update(entity);
                await UnitSave();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
            //else
            //{
            //    throw new BusinessException("Não foi possível recuperar os dados do XML da nota, para atualização dos dados.");
            //}

        }

        protected double FatorConversaoValor(double fator, double valor, TipoFatorConversao tipo)
        {
            if (tipo == TipoFatorConversao.Multiplicar)
            {
                return Math.Round((valor / fator), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                return Math.Round((valor * fator), 2, MidpointRounding.AwayFromZero);
            }
        }

        protected double FatorConversaoQuantidade(double fator, double quantidade, TipoFatorConversao tipo)
        {
            if (tipo == TipoFatorConversao.Multiplicar)
            {
                return Math.Round((quantidade * fator), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                return Math.Round((quantidade / fator), 2, MidpointRounding.AwayFromZero);
            }
        }


    }
}