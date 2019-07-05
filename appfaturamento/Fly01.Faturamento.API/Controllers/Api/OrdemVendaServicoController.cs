using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;
using System.Web.OData;
using System.Web.Http;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("ordemvendaservico")]
    public class OrdemVendaServicoController : ApiPlataformaController<OrdemVendaServico, OrdemVendaServicoBL>
    {
        [EnableQuery(PageSize = 1000, MaxTop = 1000, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }

        protected override IQueryable<OrdemVendaServico> All()
        {
            return UnitOfWork.GetGenericBL<OrdemVendaServicoBL>().All.OrderBy(x => x.DataInclusao);
        }
    }
}