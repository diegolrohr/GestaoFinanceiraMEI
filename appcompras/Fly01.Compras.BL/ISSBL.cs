using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;

namespace Fly01.Compras.BL
{
    public class ISSBL : DomainBaseBL<Iss>
    {
        public ISSBL(AppDataContext context) : base(context) { }
    }
}