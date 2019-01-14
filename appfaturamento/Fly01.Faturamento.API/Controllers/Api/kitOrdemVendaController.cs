using System;
using System.Threading.Tasks;
using System.Web.Http;
using Fly01.Faturamento.BL;
using Fly01.Core.API;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("kitordemvenda")]
    public class KitOrdemVendaController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post(UtilizarKitVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.OrdemVendaBL.UtilizarKitOrdemVenda(entity);
                await unitOfWork.Save();
            }
            return Ok(new { success = true });
        }
    }
}