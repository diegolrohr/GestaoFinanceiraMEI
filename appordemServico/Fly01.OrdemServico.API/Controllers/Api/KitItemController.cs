using Fly01.Core.Entities.Domains.Commons;
using Fly01.OrdemServico.BL;
using System.Web.OData.Routing;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [ODataRoutePrefix("kititem")]
    public class KitItemController : ApiPlataformaController<KitItem, KitItemBL>
    {
        public KitItemController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}
