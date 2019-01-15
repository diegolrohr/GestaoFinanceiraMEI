using Fly01.Compras.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class KitBL : PlataformaBaseBL<Kit>
    {
        public KitBL(AppDataContext context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }
    }
}
