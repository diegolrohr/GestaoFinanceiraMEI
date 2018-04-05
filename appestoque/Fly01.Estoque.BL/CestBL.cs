using Fly01.Estoque.DAL;
using Fly01.Estoque.Domain.Entities;
using Fly01.Core.BL;

namespace Fly01.Estoque.BL
{
    public class CestBL : DomainBaseBL<Cest>
    {
        public CestBL(AppDataContext context) : base(context) { }
    }
}
