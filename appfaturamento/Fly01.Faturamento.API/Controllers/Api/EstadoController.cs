using Fly01.Faturamento.BL;
using Fly01.Faturamento.Domain.Entities;
using System.Web.OData.Routing;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("estado")]
    public class EstadoController : ApiDomainController<Estado, EstadoBL>
    {

    }
}