using System.Web.OData.Routing;
using Fly01.Compras.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("nbs")]
    public class NBSController : ApiDomainController<Nbs, NBSBL>
    {
    }
}