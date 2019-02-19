using Fly01.Compras.BL;
using System;
using System.Web.Http;
using System.Threading.Tasks;
using Fly01.Core.API;
using System.Linq;
using Fly01.Core.Notifications;

namespace Fly01.Compras.API.Controllers.Api
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
                    if (unitOfWork.CertificadoDigitalBL.CertificadoAtualValido() == null)
                    {
                        throw new BusinessException("Cadastre o seu Certificado Digital em Configurações");
                    }
                    if (unitOfWork.ParametroTributarioBL.ParametroAtualValido() == null)
                    {
                        throw new BusinessException("Cadastre os Parâmetros Tributários em Configurações");
                    }

                    unitOfWork.MonitorNFBL.AtualizaStatusTSS(PlataformaUrl);
                    await unitOfWork.Save();
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}