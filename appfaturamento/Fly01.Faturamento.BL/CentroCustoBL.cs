using Fly01.Faturamento.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.BL
{
    public class CentroCustoBL : PlataformaBaseBL<CentroCusto>
    {
        public CentroCustoBL(AppDataContext context)
            : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }
    }
}
