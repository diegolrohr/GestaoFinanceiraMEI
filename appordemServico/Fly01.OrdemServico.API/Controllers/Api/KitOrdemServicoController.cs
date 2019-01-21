using Fly01.Core.API;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.OrdemServico.BL;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Routing;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [ODataRoutePrefix("kitordemservico")]
    public class KitOrdemServicoController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post(UtilizarKitVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.OrdemServicoBL.UtilizarKitOrdemServico(entity);
                await unitOfWork.Save();
            }
            return Ok(new { success = true });
        }
    }
}
