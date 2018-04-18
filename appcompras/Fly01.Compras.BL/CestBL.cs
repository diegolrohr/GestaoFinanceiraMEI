using Fly01.Compras.DAL;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class CestBL : DomainBaseBL<Cest>
    {
        public CestBL(AppDataContext context) : base(context) { }
    }
}