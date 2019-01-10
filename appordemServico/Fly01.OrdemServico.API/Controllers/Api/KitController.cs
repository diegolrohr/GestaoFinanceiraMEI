using Fly01.Core.Entities.Domains.Commons;
using Fly01.OrdemServico.BL;
using System.Web.OData.Routing;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [ODataRoutePrefix("kit")]
    public class KitController : ApiPlataformaController<Kit, KitBL>
    {
        public KitController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}
