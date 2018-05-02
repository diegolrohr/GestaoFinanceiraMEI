using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Faturamento.BL;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("certificadodigital")]
    public class CertificadoDigitalController : ApiPlataformaController<CertificadoDigital, CertificadoDigitalBL>
    {
        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<CertificadoDigital> model)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                if (!unitOfWork.CertificadoDigitalBL.All.Any())
                    return BadRequest("Nenhum certificado foi encontrado para esta plataforma.");

                if (model == null || key == default(Guid) || key == null)
                    return BadRequest(ModelState);

                var entity = Find(key);

                model.CopyChangedValues(entity);

                unitOfWork.CertificadoDigitalBL.ProcessEntity(entity);

                return await base.Put(entity.Id, model);
            }
        }

        public override async Task<IHttpActionResult> Post(CertificadoDigital entity)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    if (unitOfWork.CertificadoDigitalBL.All.Any())
                        return BadRequest("Já existe um certificado cadastrado para esta plataforma.");

                    entity = unitOfWork.CertificadoDigitalBL.ProcessEntity(entity);
                    return await base.Post(entity);
                }
            } 
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public override IHttpActionResult Get()
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    unitOfWork.CertificadoDigitalBL.DeleteCertificado(string.Empty);
                }

                return base.Get();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}