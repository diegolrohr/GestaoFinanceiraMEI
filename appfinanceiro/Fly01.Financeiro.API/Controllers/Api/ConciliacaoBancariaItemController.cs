using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Financeiro.Domain.Entities;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("conciliacaobancariaitem")]
    public class ConciliacaoBancariaItemController : ApiPlataformaController<ConciliacaoBancariaItem, ConciliacaoBancariaItemBL>
    {
    }
}