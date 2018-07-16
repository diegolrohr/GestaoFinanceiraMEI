﻿using Fly01.Faturamento.BL;
using System;
using System.Web.Http;
using System.Threading.Tasks;
using Fly01.Core.API;
using System.Linq;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("notaFiscalCartaCorrecaoAtualizaStatus")]
    public class NotaFiscalCartaCorrecaoAtualizaStatusController : ApiBaseController
    {
        public virtual async Task<IHttpActionResult> Get(Guid IdNotaFiscal)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    if (!unitOfWork.CertificadoDigitalBL.CertificadoAtualValido().Any())
                    {
                        throw new BusinessException("Cadastre o seu Certificado Digital em Configurações");
                    }
                    if (!unitOfWork.ParametroTributarioBL.ParametroAtualValido().Any())
                    {
                        throw new BusinessException("Cadastre os Parâmetros Tributários em Configurações");
                    }

                    unitOfWork.MonitorNFBL.AtualizaStatusTSSCartaCorrecao(PlataformaUrl, IdNotaFiscal);
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