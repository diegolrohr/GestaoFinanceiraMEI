using Fly01.EmissaoNFE.Domain;
using Fly01.Core.Domain;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class CfopBL : DomainBaseBL<Cfop>
    {
        public CfopBL(AppDataContextBase context) : base(context)
        {
        }
    }
}
