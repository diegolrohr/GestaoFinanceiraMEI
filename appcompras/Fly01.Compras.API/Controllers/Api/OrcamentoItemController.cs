using Fly01.Compras.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Web.OData;
using System.Web.Http;
using System.Linq;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("orcamentoitem")]
    public class OrcamentoItemController : ApiPlataformaController<OrcamentoItem, OrcamentoItemBL>
    {
        [EnableQuery(PageSize = 1000, MaxTop = 1000, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }
    }
}