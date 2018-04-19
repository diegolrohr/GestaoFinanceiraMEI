using Fly01.Estoque.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.BL
{
    public class CestBL : DomainBaseBL<Cest>
    {
        public CestBL(AppDataContext context) : base(context) { }
    }
}
