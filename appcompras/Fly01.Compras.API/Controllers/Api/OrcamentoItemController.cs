using Fly01.Compras.BL;
using System.Web.OData.Routing;
using Fly01.Compras.Domain.Entities;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("orcamentoitem")]
    public class OrcamentoItemController : ApiPlataformaController<OrcamentoItem, OrcamentoItemBL>
    {
    }
}