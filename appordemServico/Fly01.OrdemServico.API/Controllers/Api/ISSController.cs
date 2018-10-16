using System.Web.OData.Routing;
using Fly01.OrdemServico.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [ODataRoutePrefix("iss")]
    public class ISSController : ApiDomainController<Iss, ISSBL>
    {
    }
}