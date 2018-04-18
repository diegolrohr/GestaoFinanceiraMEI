using System.Web.Http;
using Fly01.Faturamento.BL;
using System;
using System.Threading.Tasks;
using Fly01.Core.API;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("notafiscalpdf")]
    public class NotaFiscalPDFController : ApiBaseController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var pdf = unitOfWork.NotaFiscalBL.NotaFiscalPDF(id);
                    await unitOfWork.Save();
                    return Ok(pdf);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}