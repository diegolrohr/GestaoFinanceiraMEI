using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Fly01.Faturamento.BL;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("parametrotributario")]
    public class ParametroTributarioController : ApiPlataformaController<ParametroTributario, ParametroTributarioBL>
    {
        public ParametroTributarioController()
        {
            MustProduceMessageServiceBus = true;
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<ParametroTributario> model)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                if (!unitOfWork.ParametroTributarioBL.All.Any())
                    return BadRequest("Nenhum Parametro Cadastrado anteriormente para esta plataforma.");

                if (model == null || key == default(Guid) || key == null)
                    return BadRequest(ModelState);

                var entity = Find(key);

                model.CopyChangedValues(entity);

                unitOfWork.ParametroTributarioBL.EnviaParametroTributario(entity);

                return await base.Put(entity.Id, model);
            }
        }

        public override async Task<IHttpActionResult> Post(ParametroTributario entity)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    if (unitOfWork.ParametroTributarioBL.ParametroAtualValido().Any())
                        return BadRequest("Ja existe Parametro Tributario cadastrado para esta plataforma.");

                    unitOfWork.ParametroTributarioBL.EnviaParametroTributario(entity);

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
                return Ok(UnitOfWork.ParametroTributarioBL.ParametroAtualValido());
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}