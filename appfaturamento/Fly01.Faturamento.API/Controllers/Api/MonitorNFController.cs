using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Fly01.Faturamento.BL;
using Fly01.Core.API;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("monitorNF")]
    public class MonitorNFController : ApiBaseController
    {
        private async Task ProcessaStatusTSS()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.MonitorNFBL.AtualizaStatusTSS(string.Empty);
                await unitOfWork.Save();
            }
        }

        [HttpPost]
        public IHttpActionResult AtualizaStatus()
        {
            try
            {
                var validsPlataformaUrl = new List<string>()
                {
                    "schedulerAzure3D587913-4BB8-4135-B40B-8177DE7F99F8.fly01dev.com.br",
                    "schedulerAzure40D25BFC-D5CC-4F3B-8312-B40124E02565.fly01.com.br"
                };

                if(!validsPlataformaUrl.Contains(PlataformaUrl))
                    return BadRequest("Chamada Inválida");
                        
                Task.Factory.StartNew(ProcessaStatusTSS);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}