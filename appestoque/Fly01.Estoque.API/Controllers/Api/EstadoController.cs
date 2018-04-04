using System.Web.OData.Routing;
using Fly01.Estoque.BL;
using Fly01.Estoque.Domain.Entities;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("estado")]
    public class EstadoController : ApiDomainController<Estado, EstadoBL>
    {
    
    }
}