using Fly01.EmissaoNFE.Domain;
using Fly01.Core;
using Fly01.Core.Api.BL;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.EmissaoNFE.BL
{
    public class TabelaIcmsBL : DomainBaseBL<TabelaIcms>
    {
        public TabelaIcmsBL(AppDataContextBase context) : base(context)
        {
        }
    }
}
