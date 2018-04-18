using Fly01.Faturamento.BL;
using Fly01.Faturamento.Domain.Entities;
using System.Web.OData.Routing;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using System.Web.OData;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("certificadodigital")]
    public class CertificadoDigitalController : ApiPlataformaController<CertificadoDigital, CertificadoDigitalBL>
    {
        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<CertificadoDigital> model)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
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
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
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
    }
}