using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Faturamento.DAL;
using System.Linq;

namespace Fly01.Faturamento.BL
{
    public class KitItemBL : PlataformaBaseBL<KitItem>
    {
        protected KitBL KitBL { get; set; }
        protected  ProdutoBL ProdutoBL { get; set; }
        protected ServicoBL ServicoBL { get; set; }

        public KitItemBL(AppDataContext context, KitBL kitBL, ProdutoBL produtoBL, ServicoBL servicoBL) : base(context)
        {
            KitBL = kitBL;
            ProdutoBL = produtoBL;
            ServicoBL = servicoBL;
        }

        public override void ValidaModel(KitItem entity)
        {
            entity.Fail(entity.ProdutoId == null && entity.ServicoId == null, new Error("É necessário ao menos ter um produto ou um serviço adicionado ao kit"));
            entity.Fail(All.Any(x => x.ProdutoId == entity.ProdutoId), new Error("Este produto já existe no kit"));
            entity.Fail(All.Any(x => x.ServicoId == entity.ServicoId), new Error("Este serviço já existe no kit"));

            base.ValidaModel(entity);
        }
    }
}