using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Estoque.BL;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using Fly01.Estoque.ViewModel;

namespace Fly01.Estoque.API.Controllers.Api
{
    [RoutePrefix("api/movimentoEstoque")]
    public class MovimentoEstoqueController : ApiBaseController
    {
        private Func<MovimentoEstoque, RelatorioMovimentoEstoqueVM> GetDisplayData()
        {
            return x => new RelatorioMovimentoEstoqueVM()
            {
                Id = x.Id,
                ProdutoDescricao = x.Produto?.Descricao ?? "",
                Quantidade = x.QuantidadeMovimento.HasValue ? x.QuantidadeMovimento.Value : 0.0,
                Data = x.DataInclusao,
                InventarioDescricao = x.Inventario?.Descricao,
                Observacao = x.Observacao,
                SaldoAntesMovimento = x.SaldoAntesMovimento.HasValue ? x.SaldoAntesMovimento.Value : 0.0,
                TipoMovimentoDescricao = x.TipoMovimento?.Descricao
            };
        }

        [HttpGet]
        public IHttpActionResult Get(DateTime? dataInicial,
                             DateTime? dataFinal,
                             Guid? produtoId,
                             Guid? tipoMovimentoId,
                             Guid? inventarioId)
        {

            Func<MovimentoEstoque, bool> filterPredicate = (x => (
                ((!dataInicial.HasValue) || (x.DataInclusao.Date >= dataInicial?.Date)) &&
                ((!dataFinal.HasValue) || (x.DataInclusao.Date <= dataFinal?.Date)) &&
                ((!produtoId.HasValue) || (x.ProdutoId == produtoId)) &&
                ((!tipoMovimentoId.HasValue) || (x.TipoMovimentoId == tipoMovimentoId)) &&
                ((!inventarioId.HasValue) || (x.InventarioId == inventarioId)))
            );

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                List<RelatorioMovimentoEstoqueVM> result = unitOfWork.MovimentoEstoqueBL
                   .AllIncluding(
                       x => x.Produto,
                       x => x.TipoMovimento,
                       x => x.Inventario
                   ).Where(filterPredicate)
                   .Take(2000)
                   .OrderBy(x => x.Produto.Descricao).ThenBy(x => x.DataInclusao)
                   .Select(GetDisplayData()).ToList();

                return Ok(new { value = result });
            }
        }
    }
}