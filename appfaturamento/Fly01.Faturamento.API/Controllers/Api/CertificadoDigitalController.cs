using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Faturamento.BL;
using System;
using System.Data.Entity.Infrastructure;
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
        public CertificadoDigitalController()
        {
            MustProduceMessageServiceBus = true;
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<CertificadoDigital> model)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                if (!unitOfWork.CertificadoDigitalBL.All.Any())
                    return BadRequest("Nenhum certificado foi encontrado para esta plataforma.");

                if (model == null || key == default(Guid) || key == null)
                    return BadRequest(ModelState);

                var entity = Find(key);
                entity.CertificadoValidoNFS = true;


                ModelState.Clear();
                model.Patch(entity);
                Update(entity);

                Validate(entity);

                if (!ModelState.IsValid)
                    AddErrorModelState(ModelState);

                try
                {
                    EnviaCertificadoTSS(entity);

                    await UnitSave();

                    if (MustProduceMessageServiceBus)
                        Producer<CertificadoDigital>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.EnHttpVerb.PUT);

                    if (MustExecuteAfterSave)
                        AfterSave(entity);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(key))
                        return NotFound();
                    else
                        throw;
                }

                return Ok();
            }
        }

        private void EnviaCertificadoTSS(CertificadoDigital entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.CertificadoDigitalBL.ProcessEntity(entity);
            }
        }

        public override async Task<IHttpActionResult> Post(CertificadoDigital entity)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    if (unitOfWork.CertificadoDigitalBL.CertificadoAtualValido() != null)
                        throw new Exception("Já existe um certificado cadastrado para esta plataforma.");

                    entity.CertificadoValidoNFS = true;
                    EnviaCertificadoTSS(entity);

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
                var result = UnitOfWork.CertificadoDigitalBL.CertificadoAtualValido();
                if (result != null)
                {
                    return Ok(result);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}