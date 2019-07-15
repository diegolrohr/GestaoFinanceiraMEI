using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using System.Web.OData;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Data.Entity;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;

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

            entity.Total = GetTotalPedidoItens(entity);

            Validate(entity);

            Insert(entity);

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

        private double GetTotalPedidoItens(OrdemVenda ordemVenda)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var configuracaoPersonalizacao = unitOfWork.ConfiguracaoPersonalizacaoBL.All.AsNoTracking().FirstOrDefault();
                var exibirTransportadora = configuracaoPersonalizacao != null ? configuracaoPersonalizacao.ExibirStepTransportadoraCompras : true;

                var total = unitOfWork
                                .OrdemVendaProdutoBL
                                .All
                                .Where(x => x.OrdemVendaId == ordemVenda.Id)
                                .ToList()
                                .Select(x => new
                                {
                                    Total = Convert.ToDouble(Math.Round((x.Quantidade * x.Valor) - x.Desconto, 2,
                                        MidpointRounding.AwayFromZero))
                                })
                                .Sum(x => x.Total) +
                                (((ordemVenda.TipoFrete == TipoFrete.FOB) && exibirTransportadora) ? (ordemVenda.ValorFrete ?? 0.0) : 0.0);

                total += unitOfWork
                                .OrdemVendaServicoBL
                                .All
                                .Where(x => x.OrdemVendaId == ordemVenda.Id)
                                .ToList()
                                .Select(x => new
                                {
                                    Total = Convert.ToDouble(Math.Round((x.Quantidade * x.Valor) - x.Desconto, 2,
                                        MidpointRounding.AwayFromZero))
                                })
                                .Sum(x => x.Total) +
                                (((ordemVenda.TipoFrete == TipoFrete.FOB) && exibirTransportadora) ? (ordemVenda.ValorFrete ?? 0.0) : 0.0);

                return total;
            }
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

        private async Task AtualizaTotalAoIncluirOuAlterar(OrdemVendaItem entity = null)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var configuracaoPersonalizacao = unitOfWork.ConfiguracaoPersonalizacaoBL.All.AsNoTracking().FirstOrDefault();
                var exibirTransportadora = configuracaoPersonalizacao != null ? configuracaoPersonalizacao.ExibirStepTransportadoraCompras : true;

                var pedido = unitOfWork.OrdemVendaBL.Find(entity.OrdemVendaId);
                var total = unitOfWork
                                .OrdemVendaProdutoBL
                                .All
                                .Where(x => x.Id == entity.OrdemVendaId && x.Id != entity.Id)
                                .ToList()
                                .Select(x => new
                                {
                                    Total = Convert.ToDouble(Math.Round(x.Quantidade * x.Valor - x.Desconto, 2,
                                        MidpointRounding.AwayFromZero))
                                })
                                .Sum(x => x.Total) + entity.Total;

                total = unitOfWork
                                .OrdemVendaServicoBL
                                .All
                                .Where(x => x.Id == entity.OrdemVendaId && x.Id != entity.Id)
                                .ToList()
                                .Select(x => new
                                {
                                    Total = Convert.ToDouble(Math.Round(x.Quantidade * x.Valor - x.Desconto, 2,
                                        MidpointRounding.AwayFromZero))
                                })
                                .Sum(x => x.Total) + entity.Total;
                //Criar migration criando o campo Total no OrdemVenda
                //pedido.Total += total + (((pedido.TipoFrete == TipoFrete.FOB || pedido.TipoFrete == TipoFrete.Destinatario) && exibirTransportadora) ? (pedido.ValorFrete ?? 0.0) : 0.0);
                unitOfWork.OrdemVendaBL.Update(pedido);
                await unitOfWork.Save();
            }
        }
    }
}