using Fly01.Compras.BL;
using Fly01.Core.Entities.Domains.Commons;
using System.Web.Http;
using System;
using Fly01.Core.API;
using System.Threading.Tasks;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("removeCertificado")]
    public class RemoveCertificadoController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var certificados = unitOfWork.CertificadoDigitalBL.TodosCertificados();

                    if (certificados.Count == 0)
                        throw new Exception("Não há nenhum certificado cadastrado!");

                    foreach (CertificadoDigital item in certificados)
                    {
                        unitOfWork.CertificadoDigitalBL.Delete(item);
                        await unitOfWork.Save();
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