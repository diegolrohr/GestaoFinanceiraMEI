using Fly01.Estoque.BL;
using Fly01.Estoque.Domain.Entities;
using System.Web.OData.Routing;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("ncm")]
    public class NCMController : ApiDomainController<NCM, NCMBL>
    {
    }
}