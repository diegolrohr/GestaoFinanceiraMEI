using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("danfePDF")]
    public class DanfePDFController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(DanfeVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.DanfeBL.ValidaModel(entity);
                
                try
                {
                    var pdf = (int)entity.EntidadeAmbiente == 2 ? Homologacao(entity) : Producao(entity);

                    return Ok(pdf);
                }
                catch (Exception ex)
                {
                    if (unitOfWork.EntidadeBL.TSSException(ex))
                    {
                        unitOfWork.EntidadeBL.EmissaoNFeException(ex, entity);
                    }

                    return InternalServerError(ex);
                }            
            }
        }

        public PDFVM Producao(DanfeVM entity)
        {
            var danfeId = new NFESBRAProd.IDDANFE
            {
                ID = entity.DanfeId,
                TYPE = "2",
                PART = "1"
            };
            if (entity.Logo != null)
            {
                danfeId.LOGO = Convert.FromBase64String(entity.Logo);
            }

            var response = new NFESBRAProd.NFESBRA().RETORNADANFE(AppDefault.Token, entity.Producao, danfeId);

            var pdf = new PDFVM()
            {
                PDF = response.DANFE != null ? Convert.ToBase64String(response.DANFE) : ""
            };

            return pdf;
        }

        public PDFVM Homologacao(DanfeVM entity)
        {
            var danfeId = new NFESBRA.IDDANFE
            {
                ID = entity.DanfeId,
                TYPE = "2",
                PART = "1"
            };
            if (entity.Logo != null)
            {
                danfeId.LOGO = Convert.FromBase64String(entity.Logo);
            }

            var response = new NFESBRA.NFESBRA().RETORNADANFE(AppDefault.Token, entity.Homologacao, danfeId);

            var pdf = new PDFVM()
            {
                PDF = response.DANFE != null ? Convert.ToBase64String(response.DANFE) : ""
            };

            return pdf;
        }
    }
}