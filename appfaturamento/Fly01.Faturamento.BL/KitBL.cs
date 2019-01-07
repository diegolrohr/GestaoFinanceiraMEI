using Fly01.Faturamento.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.BL
{
    public class KitBL : PlataformaBaseBL<Kit>
    {
        public KitBL(AppDataContext context) : base(context)
        { }
    }
}
