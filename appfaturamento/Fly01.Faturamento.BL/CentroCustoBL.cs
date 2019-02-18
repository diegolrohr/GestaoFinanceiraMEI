using Fly01.Faturamento.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using System.Data.Entity;
using System.Linq;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.BL
{
    public class CentroCustoBL : PlataformaBaseBL<CentroCusto>
    {
        public CentroCustoBL(AppDataContext context)
            : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(CentroCusto entity)
        {
            entity.Fail(All.AsNoTracking().Where(x => x.Descricao.ToUpper() == entity.Descricao.ToUpper()).Any(x => x.Id != entity.Id), new Error("Descrição do centro de custo já foi utilizada em outro cadastro", "descricao"));
            entity.Fail(All.AsNoTracking().Where(x => x.Codigo.ToUpper() == entity.Codigo.ToUpper()).Any(x => x.Id != entity.Id), new Error("Código do centro de custo já foi utilizado em outro cadastro", "codigo"));
            base.ValidaModel(entity);
        }
    }
}
