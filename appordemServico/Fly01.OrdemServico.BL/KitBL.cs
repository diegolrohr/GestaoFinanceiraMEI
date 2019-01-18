using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.OrdemServico.DAL;

namespace Fly01.OrdemServico.BL
{
    public class KitBL : PlataformaBaseBL<Kit>
    {
        public KitBL(AppDataContext context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }
    }
}
