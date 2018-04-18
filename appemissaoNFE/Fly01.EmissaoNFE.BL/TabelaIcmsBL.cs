using Fly01.EmissaoNFE.Domain;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.EmissaoNFE.BL
{
    public class TabelaIcmsBL : DomainBaseBL<TabelaIcms>
    {
        public TabelaIcmsBL(AppDataContextBase context) : base(context)
        {
        }
    }
}
