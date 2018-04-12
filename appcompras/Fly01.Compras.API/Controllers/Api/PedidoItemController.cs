using Fly01.Compras.BL;
using Fly01.Compras.Domain.Entities;
using Fly01.Compras.Domain.Enums;
using Fly01.Core.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("pedidoitem")]
    public class PedidoItemController : ApiPlataformaController<PedidoItem, PedidoItemBL>
    {
        public override async Task<IHttpActionResult> Post(PedidoItem entity)
        {
            try
            {
                await AtualizaTotalAoIncluirOuAlterar(entity);
                return await base.Post(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public override Task<IHttpActionResult> Put(Guid key, Delta<PedidoItem> model)
        {
            try
            {
                AtualizaTotalAoIncluirOuAlterar(model);
                return base.Put(key, model);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        private async Task AtualizaTotalAoIncluirOuAlterar(Delta<PedidoItem> model)
        {
            var entity = new PedidoItem();
            model.Patch(entity);
            await AtualizaTotalAoIncluirOuAlterar(entity);
        }

        private async Task AtualizaTotalAoIncluirOuAlterar(PedidoItem entity = null)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var pedido = unitOfWork.PedidoBL.Find(entity.PedidoId);
                var total = unitOfWork
                                .PedidoItemBL
                                .All
                                .Where(x => x.PedidoId == entity.PedidoId && x.Id != entity.Id)
                                .ToList()
                                .Select(x => new
                                {
                                    Total = Convert.ToDouble(Math.Round(x.Quantidade * x.Valor - x.Desconto, 2,
                                        MidpointRounding.AwayFromZero))
                                })
                                .Sum(x => x.Total) + entity.Total;

                pedido.Total += total + CalculaFreteACobrar(pedido);
                unitOfWork.PedidoBL.Update(pedido);
                await unitOfWork.Save();
            }
        }

        #region Excluir

        public override Task<IHttpActionResult> Delete(Guid key)
        {
            AtualizaTotalPedidoAoExcluir(key);
            return base.Delete(key);
        }

        protected override void Delete(PedidoItem primaryKey)
        {
            AtualizaTotalPedidoAoExcluir(primaryKey);
            base.Delete(primaryKey);
        }

        private async Task AtualizaTotalPedidoAoExcluir(Guid key)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var entity = unitOfWork.PedidoItemBL.Find(key);
                await AtualizaTotalPedidoAoExcluir(entity);
            }
        }

        private async Task AtualizaTotalPedidoAoExcluir(PedidoItem entity)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var pedido = unitOfWork.PedidoBL.Find(entity.PedidoId);
                var total = unitOfWork
                            .PedidoItemBL
                            .All
                            .Where(x => x.PedidoId == entity.PedidoId && x.Id != entity.Id)
                            .ToList()
                            .Select(x => new
                            {
                                Total = Convert.ToDouble(Math.Round(x.Quantidade * x.Valor - x.Desconto, 2,
                                    MidpointRounding.AwayFromZero))
                            })
                            .Sum(x => x.Total);

                pedido.Total += total + CalculaFreteACobrar(pedido);
                unitOfWork.PedidoBL.Update(pedido);
                await unitOfWork.Save();
            }
        }

        #endregion Excluir

        public static double CalculaFreteACobrar(Pedido pedido)
        {
            if (pedido.TipoFrete == TipoFrete.FOB || pedido.TipoFrete == TipoFrete.Destinatario)
                return pedido.ValorFrete ?? 0.0;

            return 0.0;
        }
    }
}