using Fly01.Faturamento.BL;
using Fly01.Faturamento.Domain.Entities;
using System.Web.OData.Routing;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("servico")]
    public class ServicoController : ApiPlataformaController<Servico, ServicoBL> { }
}