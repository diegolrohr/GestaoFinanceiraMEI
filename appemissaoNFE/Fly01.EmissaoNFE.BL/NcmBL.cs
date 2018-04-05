using Fly01.EmissaoNFE.Domain;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.EmissaoNFE.BL
{
    public class NcmBL : DomainBaseBL<NCM>
    {
        public NcmBL(AppDataContextBase context) : base(context)
        {
        }
    }
}
