using Fly01.Core.Entities.Domains.Commons;
using Fly01.Faturamento.BL;
using Fly01.Faturamento.Domain.Entities;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("estado")]
    public class EstadoController : ApiDomainController<Estado, EstadoBL>
    {

    }
}