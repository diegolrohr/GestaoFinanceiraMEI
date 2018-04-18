using Fly01.Core.Entities.Domains.Commons;
using Fly01.Faturamento.BL;
using System.Web.OData.Routing;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("estado")]
    public class EstadoController : ApiDomainController<Estado, EstadoBL>
    {

    }
}