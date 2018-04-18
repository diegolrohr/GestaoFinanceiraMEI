using Fly01.Estoque.BL;
using Fly01.Estoque.Domain.Entities;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("ncm")]
    public class NCMController : ApiDomainController<Ncm, NCMBL>
    {
    }
}