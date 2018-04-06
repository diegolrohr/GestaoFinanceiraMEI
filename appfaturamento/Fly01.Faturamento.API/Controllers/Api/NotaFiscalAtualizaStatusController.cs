using Fly01.Faturamento.BL;
using System;
using System.Web.Http;
using System.Threading.Tasks;
using Fly01.Core.API;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("notaFiscalAtualizaStatus")]
    public class NotaFiscalAtualizaStatusController : ApiBaseController
    {
        public virtual async Task<IHttpActionResult> Get()
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    unitOfWork.MonitorNFBL.AtualizaStatusTSS(PlataformaUrl);
                    await unitOfWork.Save();
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}