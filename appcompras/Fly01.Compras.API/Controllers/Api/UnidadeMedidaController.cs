using System.Web.OData.Routing;
using Fly01.Compras.BL;
using Fly01.Compras.Domain.Entities;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("unidademedida")]
    public class UnidadeMedidaController : ApiDomainController<UnidadeMedida, UnidadeMedidaBL>
    {
    }
}