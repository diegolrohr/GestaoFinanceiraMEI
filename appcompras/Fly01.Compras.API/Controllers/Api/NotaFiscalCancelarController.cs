using System.Web.Http;
using Fly01.Compras.BL;
using System;
using System.Threading.Tasks;
using Fly01.Core.API;
using Fly01.Core.Notifications;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("notafiscalcancelar")]
    public class NotaFiscalCancelarController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post(Guid id)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    unitOfWork.NotaFiscalEntradaBL.NotaFiscalCancelar(id);
                    await unitOfWork.Save();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}