using Fly01.Compras.BL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Data.Entity;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("pedidoitem")]
    public class PedidoItemController : ApiPlataformaController<PedidoItem, PedidoItemBL>
    {
        [EnableQuery(PageSize = 1000, MaxTop = 1000, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }

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
                var configuracaoPersonalizacao = unitOfWork.ConfiguracaoPersonalizacaoBL.All.AsNoTracking().FirstOrDefault();
                var exibirTransportadora = configuracaoPersonalizacao != null ? configuracaoPersonalizacao.ExibirStepTransportadoraCompras : true;

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

                pedido.Total += total + (((pedido.TipoFrete == TipoFrete.FOB || pedido.TipoFrete == TipoFrete.Destinatario) && exibirTransportadora) ? (pedido.ValorFrete ?? 0.0) : 0.0);
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
                var configuracaoPersonalizacao = unitOfWork.ConfiguracaoPersonalizacaoBL.All.AsNoTracking().FirstOrDefault();
                var exibirTransportadora = configuracaoPersonalizacao != null ? configuracaoPersonalizacao.ExibirStepTransportadoraCompras : true;

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

                pedido.Total += total + (((pedido.TipoFrete == TipoFrete.FOB || pedido.TipoFrete == TipoFrete.Destinatario) && exibirTransportadora) ? (pedido.ValorFrete ?? 0.0) : 0.0);
                unitOfWork.PedidoBL.Update(pedido);
                await unitOfWork.Save();
            }
        }

        #endregion Excluir
    }
}