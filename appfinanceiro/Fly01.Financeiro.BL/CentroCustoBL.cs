using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.API.Models.DAL;

namespace Fly01.Financeiro.BL
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
