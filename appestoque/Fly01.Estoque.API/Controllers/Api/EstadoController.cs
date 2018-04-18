using System.Web.OData.Routing;
using Fly01.Estoque.BL;
using Fly01.Estoque.Domain.Entities;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("estado")]
    public class EstadoController : ApiDomainController<Estado, EstadoBL>
    {
    
    }
}