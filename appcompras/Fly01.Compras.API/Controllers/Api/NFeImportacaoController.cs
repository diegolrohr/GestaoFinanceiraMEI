﻿using Fly01.Compras.BL;
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
using Fly01.Core.Entities.Domains;
using System.Collections.Generic;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("nfeimportacao")]
    public class NFeImportacaoController : ApiPlataformaController<NFeImportacao, NFeImportacaoBL>
    {
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
                var listProducers = new List<NFeImportacaoFinalizarProducer>();
                await FinalizarESalvarDados(entity, listProducers);
                await UnitSave();
                ProcessProducers(listProducers);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Erro ao finalizar a importação: " + ex.Message);
            }
            return Ok();
        }

        private void ProcessProducers(List<NFeImportacaoFinalizarProducer> listProducers)
        {
            foreach (var item in listProducers.Where(x => x.Entity.GetType().Name == "Pessoa"))
            {
                Producer<Pessoa>.Send(item.Entity.GetType().Name, AppUser, PlataformaUrl, (Pessoa)item.Entity, item.Verbo);
            }
            foreach (var item in listProducers.Where(x => x.Entity.GetType().Name == "Produto"))
            {
                Producer<Produto>.Send(item.Entity.GetType().Name, AppUser, PlataformaUrl, (Produto)item.Entity, item.Verbo);
            }
            foreach (var item in listProducers.Where(x => x.Entity.GetType().Name == "ContaPagar"))
            {
                Producer<ContaPagar>.Send(item.Entity.GetType().Name, AppUser, PlataformaUrl, (ContaPagar)item.Entity, item.Verbo);
            }
            foreach (var item in listProducers.Where(x => x.Entity.GetType().Name == "MovimentoPedidoCompra"))
            {
                Producer<MovimentoPedidoCompra>.Send(item.Entity.GetType().Name, AppUser, PlataformaUrl, (MovimentoPedidoCompra)item.Entity, item.Verbo);
            }
        }

        private async Task FinalizarESalvarDados(NFeImportacao entity, List<NFeImportacaoFinalizarProducer> listProducers)
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
                    entity.AtualizaDadosFornecedor = false;
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
                    listProducers.Add(new NFeImportacaoFinalizarProducer() { Entity = fornecedor, Verbo = RabbitConfig.EnHttpVerb.POST });
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
                        listProducers.Add(new NFeImportacaoFinalizarProducer() { Entity = fornecedor, Verbo = RabbitConfig.EnHttpVerb.PUT });
                    }
                }
                #endregion

                #region Transportadora

                var hasTagTransportadora = (NFe != null && NFe.InfoNFe != null && NFe.InfoNFe.Transporte != null && NFe.InfoNFe.Transporte.Transportadora != null && NFe.InfoNFe.Transporte.Transportadora?.RazaoSocial != null);
                if (hasTagTransportadora)
                {
                    if (entity.NovaTransportadora && entity.TipoFrete != TipoFrete.SemFrete)
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
                        listProducers.Add(new NFeImportacaoFinalizarProducer() { Entity = transportadora, Verbo = RabbitConfig.EnHttpVerb.POST });
                    }
                    else if (entity.AtualizaDadosTransportadora)
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
                            listProducers.Add(new NFeImportacaoFinalizarProducer() { Entity = transportadora, Verbo = RabbitConfig.EnHttpVerb.PUT });
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
                        var saldoFator = FatorConversaoQuantidade(item.FatorConversao, item.Quantidade, item.TipoFatorConversao);
                        var valorFator = FatorConversaoValor(item.FatorConversao, item.Valor, item.TipoFatorConversao);
                        var novoproduto = new Produto()
                        {
                            Id = item.ProdutoId.Value,
                            Descricao = item.Descricao,
                            CodigoBarras = item.CodigoBarras,
                            UnidadeMedidaId = item.UnidadeMedidaId,
                            ValorCusto = valorFator,
                            ValorVenda = item.AtualizaValorVenda ? item.ValorVenda : 0,
                            NcmId = UnitOfWork.NCMBL.All.FirstOrDefault(x => x.Codigo == nfeproduto.NCM)?.Id,
                            CestId = UnitOfWork.CestBL.All.FirstOrDefault(x => x.Codigo == nfeproduto.CEST)?.Id,
                            SaldoProduto = item.MovimentaEstoque ? saldoFator : 0,
                            TipoProduto = TipoProduto.Insumo
                        };
                        UnitOfWork.ProdutoBL.Insert(novoproduto);
                        UnitOfWork.NFeImportacaoProdutoBL.Update(item);
                        listProducers.Add(new NFeImportacaoFinalizarProducer() { Entity = novoproduto, Verbo = RabbitConfig.EnHttpVerb.POST });
                    }
                    else if (item.AtualizaDadosProduto || item.AtualizaValorVenda)
                    {
                        var produto = UnitOfWork.ProdutoBL.Find(item.ProdutoId);
                        if (produto != null)
                        {
                            if (item.AtualizaDadosProduto)
                            {
                                var valorFator = FatorConversaoValor(item.FatorConversao, item.Valor, item.TipoFatorConversao);
                                produto.Descricao = item.Descricao;
                                produto.CodigoBarras = item.CodigoBarras;
                                produto.ValorCusto = valorFator;
                                produto.NcmId = UnitOfWork.NCMBL.All.FirstOrDefault(x => x.Codigo == nfeproduto.NCM)?.Id;
                                produto.CestId = UnitOfWork.CestBL.All.FirstOrDefault(x => x.Codigo == nfeproduto.CEST)?.Id;
                            }
                            else if (item.AtualizaValorVenda)
                            {
                                produto.ValorVenda = item.AtualizaValorVenda ? item.ValorVenda : produto.ValorVenda;
                            }

                            UnitOfWork.ProdutoBL.Update(produto);
                            listProducers.Add(new NFeImportacaoFinalizarProducer() { Entity = produto, Verbo = RabbitConfig.EnHttpVerb.PUT });
                        }
                    }
                }
                #endregion

                #region Financeiro
                if (entity.GeraFinanceiro)
                {
                    if (entity.GeraContasXml && UnitOfWork.NFeImportacaoCobrancaBL.All.Any(x => x.NFeImportacaoId == entity.Id))
                    {
                        var condicacaoAVistaId = UnitOfWork.CondicaoParcelamentoBL.All.FirstOrDefault(x => x.QtdParcelas == 1 || x.CondicoesParcelamento == "0")?.Id;
                        if (condicacaoAVistaId == null || condicacaoAVistaId == default(Guid))
                        {
                            condicacaoAVistaId = Guid.NewGuid();
                            var novacondicaoAVista = new CondicaoParcelamento()
                            {
                                Id = condicacaoAVistaId.Value,
                                QtdParcelas = 1,
                                Descricao = "Avista"
                            };
                            UnitOfWork.CondicaoParcelamentoBL.Insert(novacondicaoAVista);
                            listProducers.Add(new NFeImportacaoFinalizarProducer() { Entity = novacondicaoAVista, Verbo = RabbitConfig.EnHttpVerb.POST });
                        }
                        foreach (var item in UnitOfWork.NFeImportacaoCobrancaBL.All.Where(x => x.NFeImportacaoId == entity.Id))
                        {
                            item.ContaFinanceiraId = Guid.NewGuid();
                            var contaPagar = new ContaPagar()
                            {
                                Id = item.ContaFinanceiraId.Value,
                                ValorPrevisto = item.Valor,
                                CategoriaId = entity.CategoriaId.Value,
                                CondicaoParcelamentoId = condicacaoAVistaId.Value,
                                PessoaId = entity.FornecedorId.Value,
                                DataEmissao = entity.DataEmissao,
                                DataVencimento = item.DataVencimento,
                                Observacao = string.Format("Conta a Pagar gerada através da importação do XML da nota {0} - {1}, aplicativo Bemacash Compras", entity.Serie, entity.Numero),
                                Descricao = string.Format("Importação do XML, nota {0} - {1}", entity.Serie, entity.Numero),
                                FormaPagamentoId = entity.FormaPagamentoId.Value,
                            };
                            UnitOfWork.NFeImportacaoCobrancaBL.Update(item);
                            listProducers.Add(new NFeImportacaoFinalizarProducer() { Entity = contaPagar, Verbo = RabbitConfig.EnHttpVerb.POST });
                        }
                    }
                    else
                    {
                        entity.ContaFinanceiraPaiId = Guid.NewGuid();
                        var contaPagar = new ContaPagar()
                        {
                            Id = entity.ContaFinanceiraPaiId.Value,
                            ValorPrevisto = entity.ValorTotal,
                            CategoriaId = entity.CategoriaId.Value,
                            CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                            PessoaId = entity.FornecedorId.Value,
                            DataEmissao = entity.DataEmissao,
                            DataVencimento = entity.DataVencimento,
                            Descricao = string.Format("Importação do XML, nota {0} - {1}", entity.Serie, entity.Numero),
                            Observacao = string.Format("Conta a Pagar gerada através da importação do XML da nota {0} - {1}, aplicativo Bemacash Compras", entity.Serie, entity.Numero),
                            FormaPagamentoId = entity.FormaPagamentoId.Value,
                        };
                        listProducers.Add(new NFeImportacaoFinalizarProducer() { Entity = contaPagar, Verbo = RabbitConfig.EnHttpVerb.POST });
                    }
                }
                #endregion

                #region MovimentaEstoque
                foreach (var item in UnitOfWork.NFeImportacaoProdutoBL.All.Where(x => x.NFeImportacaoId == entity.Id && x.MovimentaEstoque && !x.NovoProduto))
                {
                    var quantidadeFator = FatorConversaoQuantidade(item.FatorConversao, item.Quantidade, item.TipoFatorConversao);

                    listProducers.Add(new NFeImportacaoFinalizarProducer()
                    {
                        Entity = new MovimentoPedidoCompra
                        {
                            Quantidade = quantidadeFator,
                            ProdutoId = item.ProdutoId.Value,
                            UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao,
                            TipoCompra = entity.Tipo,
                            PlataformaId = PlataformaUrl,
                            Serie = entity.Serie,
                            Numero = entity.Numero,
                            IsNFeImportacao = true
                        },
                        Verbo = RabbitConfig.EnHttpVerb.POST
                    });
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
                        Data = entity.DataEmissao,
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

                    try
                    {
                        using (UnitOfWork newUnitOfWork = new UnitOfWork(ContextInitialize))
                        {
                            newUnitOfWork.PedidoBL.Insert(pedido);
                            await newUnitOfWork.Save();
                            try
                            {
                                //Atente a UnitOfWork != newUnitOfWork
                                foreach (var item in UnitOfWork.NFeImportacaoProdutoBL.All.Where(x => x.NFeImportacaoId == entity.Id))
                                {
                                    item.PedidoItemId = Guid.NewGuid();
                                    newUnitOfWork.PedidoItemBL.Insert(new PedidoItem()
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
                                newUnitOfWork.PedidoBL.Update(pedido);
                                await newUnitOfWork.Save();
                            }
                            catch (Exception ex)
                            {
                                var pedidoSalvo = newUnitOfWork.PedidoBL.All.Where(x => x.Id == pedido.Id).FirstOrDefault();
                                if (pedido != null)
                                {
                                    newUnitOfWork.PedidoBL.Delete(pedidoSalvo);
                                    await newUnitOfWork.Save();
                                }
                                throw new BusinessException("Erro ao tentar salvar os produtos do novo pedido: " + ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new BusinessException("Erro ao tentar salvar o novo pedido: " + ex.Message);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
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

    public class NFeImportacaoFinalizarProducer
    {
        public PlataformaBase Entity { get; set; }

        public RabbitConfig.EnHttpVerb Verbo { get; set; }
    }
}