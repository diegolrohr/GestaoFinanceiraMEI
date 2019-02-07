using Fly01.Core.BL;
using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.Helpers;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.Rest;
using System.Data.Entity;
using Fly01.Core.ServiceBus;

namespace Fly01.Compras.BL
{
    public class NFeImportacaoBL : PlataformaBaseBL<NFeImportacao>
    {
        protected NFeImportacaoProdutoBL NFeImportacaoProdutoBL { get; set; }
        protected NFeImportacaoCobrancaBL NFeImportacaoCobrancaBL { get; set; }
        protected PessoaBL PessoaBL { get; set; }
        protected ProdutoBL ProdutoBL { get; set; }
        protected PedidoBL PedidoBL { get; set; }
        protected PedidoItemBL PedidoItemBL { get; set; }
        protected UnidadeMedidaBL UnidadeMedidaBL { get; set; }

        public NFeImportacaoBL(AppDataContext context, NFeImportacaoProdutoBL nfeImportacaoProdutoBL, PessoaBL pessoaBL, ProdutoBL produtoBL, PedidoBL pedidoBL
          , PedidoItemBL pedidoItemBL, UnidadeMedidaBL unidadeMedidaBL, NFeImportacaoCobrancaBL nfeImportacaoCobrancaBL) : base(context)
        {
            NFeImportacaoProdutoBL = nfeImportacaoProdutoBL;
            PessoaBL = pessoaBL;
            ProdutoBL = produtoBL;
            PedidoBL = pedidoBL;
            PedidoItemBL = pedidoItemBL;
            UnidadeMedidaBL = unidadeMedidaBL;
            NFeImportacaoCobrancaBL = nfeImportacaoCobrancaBL;
        }

        public override void ValidaModel(NFeImportacao entity)
        {
            base.ValidaModel(entity);
        }

        public override void Insert(NFeImportacao entity)
        {
            entity.Status = Status.Aberto;
            entity.GeraFinanceiro = true;
            entity.NovoPedido = true;
            entity.NovoFornecedor = true;
            entity.NovaTransportadora = true;

            entity.Id = Guid.NewGuid();
            entity.Fail(string.IsNullOrEmpty(entity.Xml), new Error("Envie um xml em base64", "xml"));
            entity.Fail(string.IsNullOrEmpty(entity.XmlMd5) || entity.XmlMd5?.Length != 32, new Error("MD5 do xml inválido", "xmlMd5"));
            if (entity.IsValid() && !All.Any(x => x.XmlMd5.ToUpper() == entity.XmlMd5.ToUpper()))
            {
                LerXmlEPopularDados(entity);
            }
            else
            {
                entity.Fail(true, new Error("XMl já importado", "xmlMd5"));
            }

            base.Insert(entity);
        }

        public override void Update(NFeImportacao entity)
        {
            if (entity.Status == Status.Finalizado)
            {
                var NFe = DeserializeXMlToNFe(entity.Xml);
                VerificarPendenciasFinalizacao(entity, NFe);
                ValidaModel(entity);
            }

            base.Update(entity);
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

        private bool NotaValida(NFeVM nfe)
        {
            return (
                nfe != null &&
                nfe.InfoNFe != null &&
                nfe.InfoNFe.Identificador != null &&
                nfe.InfoNFe.Destinatario != null &&
                nfe.InfoNFe.Emitente != null &&
                nfe.InfoNFe.Total != null &&
                nfe.InfoNFe.Total.ICMSTotal != null &&
                nfe.InfoNFe.Detalhes != null &&
                !nfe.InfoNFe.Detalhes.Any(x => x.Produto == null)
            );
        }

        public NFeVM DeserializeXMlToNFe(string xml)
        {
            try
            {
                var NFe = new NFeVM();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Base64Helper.DecodificaBase64(xml));

                XmlElement xelRoot = doc.DocumentElement;
                XmlNode tagNFe = xelRoot.FirstChild;
                if (tagNFe.Name == "NFe")
                {
                    XmlSerializer ser = new XmlSerializer(typeof(NFeVM));
                    StringReader sr = new StringReader(tagNFe.OuterXml);
                    NFe = (NFeVM)ser.Deserialize(sr);
                }
                return NFe;
            }
            catch (Exception)
            {
                throw new BusinessException("Erro ao deserializar o XML: Verifique o arquivo para identificar possiveis erros de formatação.");
            }
        }

        private void VerificarPendenciasFinalizacao(NFeImportacao entity, NFeVM NFe)
        {
            if (entity.Status == Status.Finalizado)
            {
                entity.Fail(((entity.FornecedorId == null || entity.FornecedorId == default(Guid)) && !entity.NovoFornecedor), new Error("Vincule o fornecedor ou marque para adicionar um novo", "fornecedorId"));
                var pagaFrete = (entity.TipoFrete == TipoFrete.FOB || entity.TipoFrete == TipoFrete.Destinatario);
                var hasTagTransportadora = (NFe != null && NFe.InfoNFe != null && NFe.InfoNFe.Transporte != null && NFe.InfoNFe.Transporte.Transportadora != null && NFe.InfoNFe.Transporte.Transportadora?.RazaoSocial != null);

                entity.Fail(((entity.TransportadoraId == null || entity.TransportadoraId == default(Guid)) && !entity.NovaTransportadora && hasTagTransportadora), new Error("Vincule a transportadora ou marque para adicionar uma nova", "transportadoraId"));
                entity.Fail((entity.GeraFinanceiro && (entity.FormaPagamentoId == null || entity.CondicaoParcelamentoId == null || entity.CategoriaId == null || entity.DataVencimento == null || entity.ValorTotal <= 0.0)),
                    new Error("Para gerar financeiro é necessário informar forma de pagamento, condição de parcelamento, categoria, valor e data vencimento"));

                entity.Fail(NFeImportacaoProdutoBL.All.Any(x => x.NFeImportacaoId == entity.Id && (x.ProdutoId == null || x.ProdutoId == default(Guid)) && !x.NovoProduto), new Error("Vincule todos produtos, exclua ou marque para adicionar um novo", "produtoId"));
            }
        }

        public override void Delete(NFeImportacao entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != Status.Aberto, new Error("Somente importação em aberto pode ser deletada", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }

        protected void TryGetFornecedorIdFromXml(NFeImportacao entity, string cnpj)
        {
            var fornecedor = PessoaBL.All.FirstOrDefault(x => x.CPFCNPJ.ToUpper() == cnpj.ToUpper() && x.Fornecedor);
            entity.FornecedorId = fornecedor?.Id;
            entity.NovoFornecedor = (fornecedor == null);
        }

        protected void TryGetTransportadoraIdFromXml(NFeImportacao entity, string cnpj)
        {
            var transportadora = PessoaBL.All.FirstOrDefault(x => x.CPFCNPJ.ToUpper() == cnpj.ToUpper() && x.Transportadora);
            entity.TransportadoraId = transportadora?.Id;
            entity.NovaTransportadora = (transportadora == null);
        }

        public void AtualizarVinculacoes(Guid id)
        {
            var NFeImportacao = All.Where(x => x.Id == id).FirstOrDefault();
            var NFe = DeserializeXMlToNFe(NFeImportacao.Xml);

            if (NFeImportacao.FornecedorId == null || NFeImportacao.FornecedorId == default(Guid))
                TryGetFornecedorIdFromXml(NFeImportacao, NFe.InfoNFe.Emitente.Cnpj);
            if (NFeImportacao.TransportadoraId == null || NFeImportacao.TransportadoraId == default(Guid))
            {
                var transpCnpj = NFe.InfoNFe?.Transporte?.Transportadora?.CNPJ?.ToUpper();
                TryGetTransportadoraIdFromXml(NFeImportacao, transpCnpj);
            }

            Update(NFeImportacao);
            TryGetProdutoIdFromXml(id);
        }

        private void TryGetProdutoIdFromXml(Guid id)
        {
            foreach (var item in NFeImportacaoProdutoBL.All.Where(x => x.NFeImportacaoId == id && x.ProdutoId == null))
            {
                var produto = ProdutoBL.All.AsNoTracking().FirstOrDefault(x => (x.CodigoBarras.ToUpper() == item.CodigoBarras.ToUpper() && x.CodigoBarras != "SEM GTIN") || x.CodigoProduto.ToUpper() == item.Codigo.ToUpper());
                if (produto != null)
                {
                    item.ProdutoId = produto.Id;
                    item.NovoProduto = false;
                    item.FatorConversao = 0.0;
                    item.TipoFatorConversao = TipoFatorConversao.Multiplicar;
                }
                NFeImportacaoProdutoBL.Update(item);
            }
        }

        private void LerXmlEPopularDados(NFeImportacao entity)
        {
            var NFe = DeserializeXMlToNFe(entity.Xml);
            if (NotaValida(NFe))
            {
                var empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
                entity.Fail(NFe.InfoNFe.Destinatario.Cnpj.ToUpper() != empresa?.CNPJ.ToUpper(), new Error("CNPJ do destinatário da nota fiscal não corresponde aos dados da sua empresa", "cnpj"));
                entity.Fail(NFe.InfoNFe.Identificador.TipoDocumentoFiscal != TipoNota.Saida, new Error("Só é possível importar nota fiscal do tipo saída"));
                entity.Fail(NFe.InfoNFe.Versao != "4.00", new Error("Só é possível importar nota fiscal da versão 4.00"));
                entity.Fail(NFe.InfoNFe.Identificador.FinalidadeEmissaoNFe != TipoCompraVenda.Normal && NFe.InfoNFe.Identificador.FinalidadeEmissaoNFe != TipoCompraVenda.Complementar, new Error("Só é possível importar nota fiscal com finalidade normal/complementar"));

                if (entity.IsValid())
                {
                    entity.Tipo = NFe.InfoNFe.Identificador.FinalidadeEmissaoNFe;
                    TryGetFornecedorIdFromXml(entity, NFe.InfoNFe.Emitente.Cnpj);

                    var transpCnpj = NFe.InfoNFe?.Transporte?.Transportadora?.CNPJ?.ToUpper();
                    entity.TipoFrete = NFe.InfoNFe.Transporte.ModalidadeFrete;
                    TryGetTransportadoraIdFromXml(entity, transpCnpj);

                    entity.DataEmissao = NFe.InfoNFe.Identificador.Emissao;
                    entity.Serie = NFe.InfoNFe.Identificador.Serie.ToString();
                    entity.Numero = NFe.InfoNFe.Identificador.NumeroDocumentoFiscal;
                    entity.ValorFrete = NFe.InfoNFe.Total.ICMSTotal.ValorFrete;
                    entity.ValorTotal = NFe.InfoNFe.Total.ICMSTotal.ValorTotalNF;
                    entity.SomatorioDesconto = NFe.InfoNFe.Total.ICMSTotal.SomatorioDesconto;
                    entity.SomatorioICMSST = NFe.InfoNFe.Total.ICMSTotal.SomatorioICMSST;
                    entity.SomatorioIPI = NFe.InfoNFe.Total.ICMSTotal.SomatorioIPI;
                    entity.SomatorioFCPST = NFe.InfoNFe.Total.ICMSTotal.SomatorioFCPST;
                    entity.SomatorioProduto = NFe.InfoNFe.Total.ICMSTotal.SomatorioProdutos;

                    #region Vincular produtos
                    var unPadraoId = UnidadeMedidaBL.All.AsNoTracking().FirstOrDefault(x => x.Abreviacao.ToUpper() == "UN")?.Id;
                    foreach (var item in NFe.InfoNFe.Detalhes.Select(x => x.Produto))
                    {
                        var produto = ProdutoBL.All.AsNoTracking().FirstOrDefault(x => (x.CodigoBarras.ToUpper() == item.GTIN.ToUpper() && x.CodigoBarras != "SEM GTIN") || x.CodigoProduto.ToUpper() == item.Codigo.ToUpper());
                        var unidadeMedida = UnidadeMedidaBL.All.AsNoTracking().FirstOrDefault(x => x.Abreviacao.ToUpper() == item.UnidadeMedida.ToUpper());
                        var NFeImportacaoProduto = new NFeImportacaoProduto()
                        {
                            NFeImportacaoId = entity.Id,
                            Descricao = item.Descricao,
                            NovoProduto = true,
                            Codigo = item.Codigo,
                            CodigoBarras = item.GTIN,
                            Quantidade = item.Quantidade,
                            Valor = item.ValorUnitario,
                            UnidadeMedidaId = unidadeMedida != null ? unidadeMedida.Id : unPadraoId.Value,
                            FatorConversao = 0.00,//para saber que não foi atualizado
                            TipoFatorConversao = TipoFatorConversao.Multiplicar,
                            MovimentaEstoque = true,
                            AtualizaDadosProduto = true,
                            AtualizaValorVenda = true,
                            ValorVenda = 0.00,
                        };

                        if (produto != null)
                        {
                            NFeImportacaoProduto.ProdutoId = produto.Id;
                            NFeImportacaoProduto.NovoProduto = false;
                        }
                        NFeImportacaoProdutoBL.Insert(NFeImportacaoProduto);
                    }
                    #endregion

                    #region Dados de Cobrança
                    if (NFe.InfoNFe.Cobranca != null && NFe.InfoNFe.Cobranca.Duplicatas != null && NFe.InfoNFe.Cobranca.Duplicatas.Any())
                    {
                        entity.GeraContasXml = true;
                        foreach (var item in NFe.InfoNFe?.Cobranca?.Duplicatas)
                        {
                            NFeImportacaoCobrancaBL.Insert(new NFeImportacaoCobranca()
                            {
                                DataVencimento = item.Vencimento,
                                Numero = item.Numero,
                                Valor = item.ValorDuplicata,
                                NFeImportacaoId = entity.Id
                            });
                        }
                    }
                    #endregion
                }
            }
            else
            {
                throw new BusinessException("Não foi possível recuperar os dados das tags(nfeProc > NFe > infNFe) do XML da nota. Verifique se a nota esta autorizada e em formato válido.");
            }
        }
    }
}