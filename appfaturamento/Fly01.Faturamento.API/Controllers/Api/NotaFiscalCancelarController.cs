using System.Web.Http;
using Fly01.Core.Controllers.API;
using Fly01.Faturamento.BL;
using System;
using System.Threading.Tasks;
using Fly01.Core.ValueObjects;

namespace Fly01.Faturamento.API.Controllers.Api
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
                    unitOfWork.NotaFiscalBL.NotaFiscalCancelar(id);
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