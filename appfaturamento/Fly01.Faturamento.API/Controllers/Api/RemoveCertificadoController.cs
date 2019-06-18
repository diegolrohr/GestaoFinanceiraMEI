using Fly01.Faturamento.BL;
using Fly01.Core.Entities.Domains.Commons;
using System.Web.Http;
using System;
using Fly01.Core.API;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("removeCertificado")]
    public class RemoveCertificadoController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult RemoveCertificados()
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var certificados = unitOfWork.CertificadoDigitalBL.TodosCertificados();

                    foreach (CertificadoDigital item in certificados)
                    {
                        unitOfWork.CertificadoDigitalBL.Delete(item);
                        //await unitOfWork.Save();
                    }
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}