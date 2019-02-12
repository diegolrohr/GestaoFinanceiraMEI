using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Compras.DAL;

namespace Fly01.Compras.BL
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
