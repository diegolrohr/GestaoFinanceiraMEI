using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using System.Linq;
using Fly01.EmissaoNFE.Domain;

namespace Fly01.EmissaoNFE.BL
{
    public class SiafiBL : DomainBaseBL<Siafi>
    {
        public SiafiBL(AppDataContextBase context) : base(context){}

        public void RetornaSiafi(ParametroVM entity)
        {
            var siafi = All.Where(x => x.CodigoIbge == entity.CodigoIBGECidade).FirstOrDefault();
            entity.Siafi = siafi.CodigoSiafi;
        } 
    }
}
