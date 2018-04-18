using System.Web.OData.Routing;
using Fly01.Faturamento.BL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("nbs")]
    public class NBSController : ApiDomainController<NBS, NBSBL>
    {
    }
}