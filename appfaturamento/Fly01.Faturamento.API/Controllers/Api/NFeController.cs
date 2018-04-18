using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Faturamento.Domain.Entities;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("nfe")]
    public class NFeController : ApiPlataformaController<NFe, NFeBL>
    {
    }
}