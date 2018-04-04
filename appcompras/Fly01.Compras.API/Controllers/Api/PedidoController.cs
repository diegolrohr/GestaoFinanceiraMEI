using Fly01.Compras.BL;
using System.Web.OData.Routing;
using Fly01.Compras.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.Http.ModelBinding;
using System.Linq;
using Fly01.Core.ValueObjects;
using Fly01.Core.Notifications;
using System.Data.Entity.Infrastructure;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("pedido")]
    public class PedidoController : ApiPlataformaController<Pedido, PedidoBL>
    {
        public PedidoController()
        {
            MustProduceMessageServiceBus = true;
        }

        public override async Task<IHttpActionResult> Post(Pedido entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            ModelState.Clear();

            Insert(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            await UnitSave();

            return Created(entity);
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Pedido> model)
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