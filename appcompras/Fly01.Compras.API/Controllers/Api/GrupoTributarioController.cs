using System.Web.OData.Routing;
using Fly01.Compras.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("grupotributario")]
    public class GrupoTributarioController : ApiPlataformaController<GrupoTributario, GrupoTributarioBL>
    {
    }
}