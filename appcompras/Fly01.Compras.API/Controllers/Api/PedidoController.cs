using Fly01.Compras.BL;
using Fly01.Core.Notifications;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.OData;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

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

            entity.Total = GetTotalPedidoItens(entity);

            Insert(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            await UnitSave();

            return Created(entity);
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Pedido> model)
        {
            if (model == null || key == default(Guid))
                return BadRequest(ModelState);

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                return NotFound();

            ModelState.Clear();
            model.Patch(entity);

            entity.Total = GetTotalPedidoItens(entity);

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

        private double GetTotalPedidoItens(Pedido pedido)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var total = unitOfWork
                                .PedidoItemBL
                                .All
                                .Where(x => x.PedidoId == pedido.Id)
                                .ToList()
                                .Select(x => new
                                {
                                    Total = Convert.ToDouble(Math.Round(x.Quantidade * x.Valor - x.Desconto, 2,
                                        MidpointRounding.AwayFromZero))
                                })
                                .Sum(x => x.Total) + PedidoItemController.CalculaFreteACobrar(pedido);
                return total;
            }
        }
    }
}