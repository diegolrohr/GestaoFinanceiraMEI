using Fly01.Compras.BL;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using System.Data.Entity;

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
            if (model == null || key == default(Guid) || key == null)
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
                else
                    throw;
            }

            return Ok(); 
        }

        private double GetTotalPedidoItens(Pedido pedido)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var configuracaoPersonalizacao = unitOfWork.ConfiguracaoPersonalizacaoBL.All.AsNoTracking().FirstOrDefault();
                var exibirTransportadora = configuracaoPersonalizacao != null ? configuracaoPersonalizacao.ExibirStepTransportadoraCompras : true;

                var total = unitOfWork
                                .PedidoItemBL
                                .All
                                .Where(x => x.PedidoId == pedido.Id)
                                .ToList()
                                .Select(x => new
                                {
                                    Total = Convert.ToDouble(Math.Round((x.Quantidade * x.Valor) - x.Desconto, 2,
                                        MidpointRounding.AwayFromZero))
                                })
                                .Sum(x => x.Total) +
                                (((pedido.TipoFrete == TipoFrete.FOB) && exibirTransportadora) ? (pedido.ValorFrete ?? 0.0) : 0.0);
                return total;
            }
        }
    }
}