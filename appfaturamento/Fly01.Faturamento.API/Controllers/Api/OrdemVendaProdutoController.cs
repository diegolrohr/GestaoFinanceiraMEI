using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Web.OData;
using System.Web.Http;
using System.Linq;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("ordemvendaproduto")]
    public class OrdemVendaProdutoController : ApiPlataformaController<OrdemVendaProduto, OrdemVendaProdutoBL>
    {
        [EnableQuery(PageSize = 1000, MaxTop = 1000, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }
    }
}