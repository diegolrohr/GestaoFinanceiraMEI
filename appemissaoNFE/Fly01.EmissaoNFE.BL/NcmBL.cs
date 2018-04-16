using Fly01.EmissaoNFE.Domain;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class NcmBL : DomainBaseBL<NCM>
    {
        public NcmBL(AppDataContextBase context) : base(context)
        {
        }
    }
}
