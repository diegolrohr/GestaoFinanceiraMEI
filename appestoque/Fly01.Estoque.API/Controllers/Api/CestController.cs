using Fly01.Estoque.BL;
using Fly01.Estoque.Domain.Entities;
using System.Web.OData.Routing;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("cest")]
    public class CestController : ApiDomainController<Cest, CestBL>
    {
        public CestController() 
        {
            var test = true;
        }
    }
}