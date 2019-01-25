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

namespace Fly01.Compras.BL
{
    public class NFeImportacaoBL : PlataformaBaseBL<NFeImportacao>
    {
        protected NFeImportacaoProdutoBL NFeImportacaoProdutoBL { get; set; }
        protected PessoaBL PessoaBL { get; set; }
        protected ProdutoBL ProdutoBL { get; set; }
        protected PedidoBL PedidoBL { get; set; }
        protected PedidoItemBL PedidoItemBL { get; set; }

        public NFeImportacaoBL(AppDataContext context, NFeImportacaoProdutoBL nfeImportacaoProdutoBL, PessoaBL pessoaBL, ProdutoBL produtoBL) : base(context)
        //, PedidoBL pedidoBL, PedidoItemBL pedidoItemBL
        {
            NFeImportacaoProdutoBL = nfeImportacaoProdutoBL;
            PessoaBL = pessoaBL;
            ProdutoBL = produtoBL;
            //PedidoBL = pedidoBL;
            //PedidoItemBL = pedidoItemBL;
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

            entity.Id = Guid.NewGuid();//para vincular já vincular os produtos
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
                VerificarPendenciasFinalizacao(entity);
                ValidaModel(entity);
                if (entity.IsValid())
                {
                    FinalizarESalvarDados(entity);
                }
            }

            base.Update(entity);
        }

        private void FinalizarESalvarDados(NFeImportacao entity)
        {
            var NFe = DeserializeXMlToNFe(entity.Xml);
            if (NotaValida(NFe))
            {
                if (entity.NovoFornecedor)
                {
                    entity.FornecedorId = new Guid();
                    PessoaBL.Insert(new Pessoa()
                    {
                        Id = entity.FornecedorId.Value,
                    });
                }
                else if (entity.AtualizaDadosFornecedor)
                {
                    entity.NovoFornecedor = false;
                    var fornecedor = PessoaBL.Find(entity.FornecedorId);
                    if (fornecedor != null)
                    {
                        fornecedor.NomeComercial = "";
                        PessoaBL.Update(fornecedor);
                    }
                    else
                    {
                        throw new BusinessException("Fornecor não localizado para atualização dos dados");
                    }
                }
            }
            else
            {
                throw new BusinessException("Não foi possível recuperar os dados do XML da nota, para atualização dos dados");
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
                nfe.InfoNFe.Detalhes.Any(x => x.Produto == null)
            );
        }

        private NFeVM DeserializeXMlToNFe(string xml)
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
            catch (Exception ex)
            {
                throw new BusinessException("Erro ao deserializar o XML: " + ex.Message);
            }
        }

        private void VerificarPendenciasFinalizacao(NFeImportacao entity)
        {
            if (entity.Status == Status.Finalizado)
            {
                entity.Fail((entity.FornecedorId == null || entity.FornecedorId == default(Guid) && !entity.NovoFornecedor), new Error("Vincule o fornecedor ou marque para adicionar um novo", "fornecedorId"));

                //TODO confirmar com Fraga
                var pagaFrete = (entity.TipoFrete == TipoFrete.FOB || entity.TipoFrete == TipoFrete.Destinatario);
                entity.Fail((entity.TransportadoraId == null || entity.TransportadoraId == default(Guid) && !entity.NovaTransportadora && entity.GeraFinanceiro), new Error("Vincule a transportadora ou marque para adicionar uma nova", "transportadoraId"));
                entity.Fail((entity.GeraFinanceiro && (entity.FormaPagamentoId == null || entity.CondicaoParcelamentoId == null || entity.CategoriaId == null || entity.DataVencimento == null || entity.ValorTotal <= 0.0)),
                    new Error("Para gerar financeiro é necessário informar forma de pagamento, condição de parcelamento, categoria, valor e data vencimento"));

                entity.Fail(NFeImportacaoProdutoBL.All.Any(x => x.NFeImportacaoId == entity.Id && (x.ProdutoId == null || x.ProdutoId == default(Guid))), new Error("Vincule todos produtos, exclua ou marque para adicionar um novo", "produtoId"));
            }
        }

        public override void Delete(NFeImportacao entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != Status.Aberto, new Error("Somente importação em aberto pode ser deletada", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }

        private void LerXmlEPopularDados(NFeImportacao entity)
        {
            var NFe = DeserializeXMlToNFe(entity.Xml);
            if (NotaValida(NFe))
            {
                var empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);

                entity.Fail(NFe.InfoNFe.Destinatario.Cnpj.ToUpper() != empresa.CNPJ.ToUpper(), new Error("Cnpj do destinatário da nota fiscal não é para o seu cnpj", "cnpj"));
                entity.Fail(NFe.InfoNFe.Identificador.TipoDocumentoFiscal != TipoNota.Saida, new Error("Só é possível importar nota fiscal do tipo saída"));
                entity.Fail(NFe.InfoNFe.Versao != "4.00", new Error("Só é possível importar nota fiscal da versão 4.00"));
                entity.Fail(entity.Tipo != TipoCompraVenda.Normal && entity.Tipo != TipoCompraVenda.Complementar, new Error("Só é possível importar nota fiscal com finalidade normal/complementar"));

                if (entity.IsValid())
                {
                    var fornecedor = PessoaBL.All.FirstOrDefault(x => x.CPFCNPJ.ToUpper() == NFe.InfoNFe.Emitente.Cnpj.ToUpper());
                    entity.FornecedorId = fornecedor?.Id;
                    entity.NovoFornecedor = (fornecedor == null);//TODO: confirmar

                    var transportadora = PessoaBL.All.FirstOrDefault(x => x.CPFCNPJ.ToUpper() == NFe.InfoNFe.Transporte.Transportadora.CNPJ.ToUpper());
                    entity.TransportadoraId = transportadora?.Id;
                    entity.TipoFrete = NFe.InfoNFe.Transporte.ModalidadeFrete;
                    entity.NovaTransportadora = (transportadora == null);

                    entity.ValorFrete = NFe.InfoNFe.Total.ICMSTotal.ValorFrete;
                    entity.ValorTotal = NFe.InfoNFe.Total.ICMSTotal.ValorTotalNF;
                    entity.SomatorioDesconto = NFe.InfoNFe.Total.ICMSTotal.SomatorioDesconto;
                    entity.SomatorioICMSST = NFe.InfoNFe.Total.ICMSTotal.SomatorioICMSST;
                    entity.SomatorioIPI = NFe.InfoNFe.Total.ICMSTotal.SomatorioIPI;
                    entity.SomatorioFCPST = NFe.InfoNFe.Total.ICMSTotal.SomatorioFCPST;
                    entity.SomatorioProduto = NFe.InfoNFe.Total.ICMSTotal.SomatorioProdutos;
                }
            }
            else
            {
                throw new BusinessException("Não foi possível recuperar os dados do XML da nota, para inclusão dos dados");
            }
        }
    }
}