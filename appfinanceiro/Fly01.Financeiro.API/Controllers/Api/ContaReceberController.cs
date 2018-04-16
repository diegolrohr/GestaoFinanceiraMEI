using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Financeiro.Domain.Entities;
using System.Web.OData;
using System.Web.Http;
using System.Linq;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("contareceber")]
    public class ContaReceberController : ApiPlataformaController<ContaReceber, ContaReceberBL>
    {
        [EnableQuery(PageSize = 1000, MaxTop = 1000, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }
    }
}