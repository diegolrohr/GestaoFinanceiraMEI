using System.Web.OData.Routing;
using Fly01.Faturamento.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("unidademedida")]
    public class UnidadeMedidaController : ApiDomainController<UnidadeMedida, UnidadeMedidaBL>
    {
    }
}