using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Faturamento.DAL;

namespace Fly01.Faturamento.BL
{
    public class KitItemBL : PlataformaBaseBL<KitItem>
    {
        public KitItemBL(AppDataContext context) : base(context)
        {
        }
    }
}