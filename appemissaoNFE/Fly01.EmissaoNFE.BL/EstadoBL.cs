using Fly01.EmissaoNFE.DAL;
using Fly01.EmissaoNFE.Domain;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.EmissaoNFE.BL
{
    public class EstadoBL : DomainBaseBL<Estado>
    {
        public EstadoBL(AppDataContext context) : base(context)
        { }
    }
}
