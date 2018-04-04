using System.Web.OData.Routing;
using Fly01.Compras.BL;
using Fly01.Compras.Domain.Entities;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("cest")]
    public class CestController : ApiDomainController<Cest, CestBL>
    {
    }
}