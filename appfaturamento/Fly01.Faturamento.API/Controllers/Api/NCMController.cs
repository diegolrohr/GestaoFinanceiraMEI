using System.Web.OData.Routing;
using Fly01.Faturamento.BL;
using Fly01.Faturamento.Domain.Entities;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("ncm")]
    public class NCMController : ApiDomainController<NCM, NCMBL>
    {
    }
}