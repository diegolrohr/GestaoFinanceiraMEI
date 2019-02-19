﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.Compras.BL;
using Fly01.Core.ServiceBus;
using System.Data.Entity.Infrastructure;

namespace Fly01.Compras.API.Controllers.Api
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

                entity.ParametroValidoNFS = true;

                ModelState.Clear();
                model.Patch(entity);
                Update(entity);

                Validate(entity);

                if (!ModelState.IsValid)
                    AddErrorModelState(ModelState);

                try
                {
                    Parallel.Invoke(() =>
                    {
                        EnviaParametrosTSS(entity);
                    });

                    await UnitSave();

                    if (MustProduceMessageServiceBus)
                        Producer<ParametroTributario>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.EnHttpVerb.PUT);

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

        private void EnviaParametrosTSS(ParametroTributario entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.ParametroTributarioBL.EnviaParametroTributario(entity);
            }
        }

        public override async Task<IHttpActionResult> Post(ParametroTributario entity)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    if (unitOfWork.ParametroTributarioBL.ParametroAtualValido() != null)
                        return BadRequest("Ja existe Parametro Tributario cadastrado para esta plataforma.");

                    entity.ParametroValidoNFS = true;

                    Parallel.Invoke(() =>
                    {
                        EnviaParametrosTSS(entity);
                    });

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
                var result = UnitOfWork.ParametroTributarioBL.ParametroAtualValido();
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