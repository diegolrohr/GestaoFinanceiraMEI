using Fly01.Compras.BL;
using Fly01.Core.API;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("kitpedido")]
    public class KitPedidoController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post(UtilizarKitVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.PedidoBL.UtilizarKitPedido(entity);
                await unitOfWork.Save();
            }
            return Ok(new { success = true });
        }
    }
}