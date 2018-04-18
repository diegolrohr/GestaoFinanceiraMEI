using System.Web.OData.Routing;
using Fly01.Faturamento.BL;
using Fly01.Faturamento.Domain.Entities;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("grupotributario")]
    public class GrupoTributarioController : ApiPlataformaController<GrupoTributario, GrupoTributarioBL>
    {
    }
}