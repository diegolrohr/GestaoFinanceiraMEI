using Fly01.Core.Entities.Domains.Commons;
using Fly01.Estoque.BL;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("movimentoestoque")]
    public class MovimentoEstoqueController : ApiPlataformaController<MovimentoEstoque, MovimentoEstoqueBL>
    {
        [EnableQuery(PageSize = 1000, MaxTop = 1000, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }
    }
}