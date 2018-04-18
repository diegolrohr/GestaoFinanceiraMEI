using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Faturamento.BL;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("grupotributario")]
    public class GrupoTributarioController : ApiPlataformaController<GrupoTributario, GrupoTributarioBL>
    {
    }
}