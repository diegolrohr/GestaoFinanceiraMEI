using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Faturamento.Domain.Entities;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using System.Web.OData;
using Fly01.Core.Notifications;
using System.Web.Http.ModelBinding;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("ordemvenda")]
    public class OrdemVendaController : ApiPlataformaController<OrdemVenda, OrdemVendaBL>
    {
        public OrdemVendaController()
        {
            MustProduceMessageServiceBus = true;
        }

        public override async Task<IHttpActionResult> Post(OrdemVenda entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            ModelState.Clear();

            Insert(entity);

            Validate(entity);

            await UnitSave();

            return Created(entity);
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<OrdemVenda> model)
        {
            if (model == null || key == default(Guid) || key == null)
                return BadRequest(ModelState);

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                return NotFound();

            ModelState.Clear();
            model.Patch(entity);
            Update(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            try
            {
                await UnitSave();

                if (MustProduceMessageServiceBus)
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

        public override async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            if (key == default(Guid) || key == null)
                return BadRequest();

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                return NotFound();

            Delete(entity);

            await UnitSave();

            return StatusCode(HttpStatusCode.NoContent);
        }

        private Notification Notification { get; } = new Notification();

        private void AddErrorModelState(ModelStateDictionary modelState)
        {
            modelState.ToList().ForEach(
                model => model.Value.Errors.ToList().ForEach(
                    itemErro => Notification.Errors.Add(
                        new Error(itemErro.ErrorMessage, string.Concat(char.ToLowerInvariant(model.Key[0]), model.Key.Substring(1))))));

            throw new BusinessException(Notification.Get());
        }
    }
}