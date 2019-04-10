using Fly01.Core.BL;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.EmissaoNFE.BL
{
    public class PaisBL : DomainBaseBL<Pais>
    {
        public PaisBL(DbContext context) : base(context)
        {
        }
    }
}
