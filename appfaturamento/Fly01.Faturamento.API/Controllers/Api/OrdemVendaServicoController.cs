using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("ordemvendaservico")]
    public class OrdemVendaServicoController : ApiPlataformaController<OrdemVendaServico, OrdemVendaServicoBL>
    {
        protected override IQueryable<OrdemVendaServico> All()
        {
            return UnitOfWork.GetGenericBL<OrdemVendaServicoBL>().All.OrderBy(x => x.DataInclusao);
        }
    }
}