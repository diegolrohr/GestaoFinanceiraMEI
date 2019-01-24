using Fly01.Core.BL;
using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Helpers;
using Fly01.EmissaoNFE.Domain.Entities.NFe;

namespace Fly01.Compras.BL
{
    public class NFeImportacaoBL : PlataformaBaseBL<NFeImportacao>
    {
        protected NFeImportacaoProdutoBL NFeImportacaoProdutoBL { get; set; }
        protected PessoaBL PessoaBL { get; set; }
        protected ProdutoBL ProdutoBL { get; set; }
        protected PedidoBL PedidoBL { get; set; }
        protected PedidoItemBL PedidoItemBL { get; set; }

        public NFeImportacaoBL(AppDataContext context, NFeImportacaoProdutoBL nfeImportacaoProdutoBL, PessoaBL pessoaBL, ProdutoBL produtoBL, PedidoBL pedidoBL, PedidoItemBL pedidoItemBL) : base(context)
        {
            NFeImportacaoProdutoBL = nfeImportacaoProdutoBL;
            PessoaBL = pessoaBL;
            ProdutoBL = produtoBL;
            PedidoBL = pedidoBL;
            PedidoItemBL = pedidoItemBL;
        }

        public override void ValidaModel(NFeImportacao entity)
        {
            base.ValidaModel(entity);
        }

        public override void Insert(NFeImportacao entity)
        {
            entity.Status = Status.Aberto;
            entity.Id = new Guid();//para vincular já vincular os produtos
            entity.Fail(string.IsNullOrEmpty(entity.Xml), new Error("Envie um xml em base64", "xml"));
            entity.Fail(string.IsNullOrEmpty(entity.XmlMd5) || entity.XmlMd5?.Length != 32, new Error("MD5 do xml inválido", "xmlMd5"));
            if(!All.Any(x => x.XmlMd5.ToUpper() == entity.XmlMd5.ToUpper()))
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
                    CriarRegistrosEAtualizarDados(entity);
                }
            }

            base.Update(entity);
        }

        private void CriarRegistrosEAtualizarDados(NFeImportacao entity)
        {
            var NFe = JsonConvert.DeserializeObject<NFeVM>(Base64Helper.DecodificaBase64(entity.Json));
            if (NFe != null)
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
                    }
                    else
                    {
                        throw new BusinessException("Fornecor não localizado para atualização dos dados");
                    }
                }

            }
            else
            {
                throw new BusinessException("Não foi possível recuperar os dados do XML(Json) da nota, para atualização dos dados");
            }
        }

        private void VerificarPendenciasFinalizacao(NFeImportacao entity)
        {
            if(entity.Status == Status.Finalizado)
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

        public override void AfterSave(NFeImportacao entity)
        {
            //VerificarPendencias(entity); referencia acho que não precisa novamente
            if (entity.IsValid() && (entity.Id != default(Guid) && entity.Id != null) && entity.Status == Status.Finalizado)
            {
                if(entity.FornecedorId != null)
                {

                }
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
            //TODO:
            //entity.Json = utilizar micro servico
            var NFe = JsonConvert.DeserializeObject<NFeVM>(Base64Helper.DecodificaBase64(entity.Json));
            if (NFe != null)
            {
                var fornecedor = PessoaBL.All.FirstOrDefault(x => x.CPFCNPJ.ToUpper() == NFe.InfoNFe.Emitente.Cnpj.ToUpper());
                    //produtos
            }
            else
            {
                entity.Fail(true, new Error("Erro ao ler dados do XMl", "json"));
            }
        }
    }
}