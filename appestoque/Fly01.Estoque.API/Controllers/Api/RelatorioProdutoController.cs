using Fly01.Core.API;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using System;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Estoque.BL;
using System.Collections.Generic;
using Fly01.Estoque.Models.ViewModel;
using System.Linq;
using Fly01.Core;

namespace Fly01.Estoque.API.Controllers.Api
{
    [RoutePrefix("api/relatorioProduto")]
    public class RelatorioProdutoController : ApiBaseController
    {
        public IHttpActionResult Get(string descricao,
                                string codigo,
                                string tipoProduto,
                                string origemMercadoria,
                                string imprimirQuantidade,
                                string imprimirValorCusto,
                                string imprimirValorVenda,
                                string imprimirNCM,
                                string imprimirEnquadramentoIPI,
                                string imprimirOrigemMercadoria,
                                Guid? grupoProdutoId,
                                Guid? unidadeMedidaId,
                                Guid? ncmId,
                                Guid? enquadramentoLegalIPIId)
        {

            Func<Produto, bool> filterPredicate = (x => (
                ((string.IsNullOrWhiteSpace(descricao)) || (x.Descricao.Contains(descricao))) &&
                ((string.IsNullOrWhiteSpace(codigo)) || (x.CodigoProduto.Contains(codigo))) &&
                ((string.IsNullOrWhiteSpace(tipoProduto)) || (x.TipoProduto == (TipoProduto)Enum.Parse(typeof(TipoProduto), tipoProduto, true))) &&
                ((!grupoProdutoId.HasValue) || (x.GrupoProdutoId == grupoProdutoId)) &&
                ((!unidadeMedidaId.HasValue) || (x.UnidadeMedidaId == unidadeMedidaId)) &&
                ((!ncmId.HasValue) || (x.NcmId == ncmId)) &&
                ((!enquadramentoLegalIPIId.HasValue) || (x.EnquadramentoLegalIPIId == enquadramentoLegalIPIId)) &&
                ((string.IsNullOrWhiteSpace(origemMercadoria)) || (x.OrigemMercadoria == (OrigemMercadoria)Enum.Parse(typeof(OrigemMercadoria), origemMercadoria, true))))
            );

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                List<RelatorioProdutoVM> result = unitOfWork.ProdutoBL
                .AllIncluding(
                    x => x.GrupoProduto,
                    x => x.UnidadeMedida,
                    x => x.Ncm,
                    x => x.EnquadramentoLegalIPI
                )
                .Where(filterPredicate)
                .Take(2000)
                .OrderBy(x => x.Descricao)
                .Select(GetDisplayData(imprimirQuantidade, imprimirValorCusto, imprimirValorVenda, imprimirNCM, imprimirEnquadramentoIPI, imprimirOrigemMercadoria)).ToList();

                return Ok(new { count = result.Count, value = result });
            }
        }

        private Func<Produto, RelatorioProdutoVM> GetDisplayData(string imprimirQuantidade, string imprimirValorCusto, string imprimirValorVenda, string imprimirNCM, string imprimirEnquadramentoIPI, string imprimirOrigemMercadoria)
        {
            return x => new RelatorioProdutoVM()
            {
                //Id = x.Id,
                Descricao = x.Descricao??"",
                Codigo = x.CodigoProduto?? "",
                TipoProduto = EnumHelper.GetDescription(typeof(TipoProduto), x.TipoProduto.ToString()),
                GrupoProduto = x.GrupoProduto?.Descricao?? "",
                UnidadeMedida = x.UnidadeMedida?.Descricao?? "",
                Ncm = x.Ncm?.Codigo?? "",
                EnquadramentoLegalIPI = x.EnquadramentoLegalIPI?.Codigo?? "",
                OrigemMercadoria = EnumHelper.GetEnumDescription(x.OrigemMercadoria),
                Quantidade = x.SaldoProduto.HasValue ? x.SaldoProduto.Value.ToString("C", AppDefaults.CultureInfoDefault).Replace("R$", "").Replace("R$ ", "") : "0,00",
                ValorVenda = x.ValorVenda.ToString("C", AppDefaults.CultureInfoDefault).Replace("R$","").Replace("R$ ",""),
                ValorCusto = x.ValorCusto.ToString("C", AppDefaults.CultureInfoDefault).Replace("R$", "").Replace("R$ ", ""),
                ImprimirEnquadramentoIPI = imprimirEnquadramentoIPI,
                ImprimirNCM = imprimirNCM,
                ImprimirOrigemMercadoria = imprimirOrigemMercadoria,
                ImprimirQuantidade = imprimirQuantidade,
                ImprimirValorCusto = imprimirValorCusto,
                ImprimirValorVenda = imprimirValorVenda
            };
        }
    }
}