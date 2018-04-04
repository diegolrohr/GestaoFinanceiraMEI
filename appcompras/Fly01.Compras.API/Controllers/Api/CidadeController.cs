using Fly01.Compras.BL;
using Fly01.Compras.Domain.Entities;
using System.Web.OData.Routing;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("cidade")]
    public class CidadeController : ApiDomainController<Cidade, CidadeBL>
    {

    }
}