using Fly01.Core.Entities.Domains.Commons;
using Fly01.OrdemServico.BL;
using System.Web.OData.Routing;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [ODataRoutePrefix("nbs")]
    public class NBSController : ApiDomainController<Nbs, NBSBL>
    {
    }
}
