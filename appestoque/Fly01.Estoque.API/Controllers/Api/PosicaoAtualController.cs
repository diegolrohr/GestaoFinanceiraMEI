using Fly01.Estoque.BL;
using Fly01.Estoque.Domain.Entities;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("posicaoatual")]
    public class PosicaoAtualController : ApiPlataformaController<PosicaoAtual, PosicaoAtualBL>
    {
        [EnableQuery(PageSize = 50, MaxTop = 50, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(UnitOfWork.PosicaoAtualBL.Get());
        }
    }
}