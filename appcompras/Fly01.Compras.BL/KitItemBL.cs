using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Compras.DAL;
using System;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Compras.BL
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
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(KitItem entity)
        {
            entity.Fail((entity.ProdutoId == null || entity.ProdutoId == default(Guid)) && (entity.ServicoId == null || entity.ServicoId == default(Guid)), new Error("É necessário ao menos ter um produto ou um serviço adicionado ao kit"));
            entity.Fail(entity.ProdutoId != null && entity.ServicoId != null, new Error("É permitido adicionar 1 produto ou 1 serviço por registro"));
            entity.Fail(All.AsNoTracking().Any(x => x.KitId == entity.KitId && x.ProdutoId == entity.ProdutoId && entity.ProdutoId != null && x.Id != entity.Id), new Error("Este produto já existe no kit"));
            entity.Fail(All.AsNoTracking().Any(x => x.KitId == entity.KitId && x.ServicoId == entity.ServicoId && entity.ServicoId != null && x.Id != entity.Id), new Error("Este serviço já existe no kit"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser superior a zero"));

            base.ValidaModel(entity);
        }
    }
}