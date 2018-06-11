using System;
using Fly01.Core;
using System.Web.Mvc;
using Fly01.Core.Helpers;
using System.Collections.Generic;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Estoque.Controllers
{
    public class ProdutoController : ProdutoBaseController<ProdutoVM>
    {
        public JsonResult GridLoadPos(Dictionary<string, string> filters = null)
        {
            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoProduto = x.CodigoProduto,
                descricao = x.Descricao,
                unidadeMedidaId = x.UnidadeMedidaId,
                unidadeMedida_descricao = x.UnidadeMedida != null ? x.UnidadeMedida.Descricao : "",
                valorCusto = x.ValorCusto.ToString("C", AppDefaults.CultureInfoDefault),
                valorVenda = x.ValorVenda.ToString("C", AppDefaults.CultureInfoDefault),
                saldoProduto = x.SaldoProduto,
                custoTotal = Convert.ToDouble(x.ValorCusto * x.SaldoProduto).ToString("C", AppDefaults.CultureInfoDefault),
                vendaTotal = Convert.ToDouble(x.ValorVenda * x.SaldoProduto).ToString("C", AppDefaults.CultureInfoDefault)
            };

            SelectProperties = "id,descricao,codigoProduto,unidadeMedidaId,valorVenda,valorCusto,saldoProduto";
            return GridLoad(filters);
        }

        public JsonResult GridLoadSaldoZerado(Dictionary<string, string> filters = null)
        {
            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                descricao = x.Descricao,
                saldoProduto = x.SaldoProduto
            };

            SelectProperties = "id,descricao,saldoProduto";

            filters.AddParam("saldoProduto", "eq 0");

            return GridLoad(filters);
        }

        public JsonResult GridLoadSaldoAbaixoMinimo(Dictionary<string, string> filters = null)
        {
            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoProduto = x.CodigoProduto,
                descricao = x.Descricao,
                saldoProduto = x.SaldoProduto
            };

            SelectProperties = "id,descricao,codigoProduto,saldoProduto";

            filters.AddParam("saldoProduto", "lt saldoMinimo");

            return GridLoad(filters);
        }

        public override List<TooltipUI> GetHelpers()
        {
            return new List<TooltipUI> {
                new TooltipUI
                {
                    Id = "codigoBarras",
                    Tooltip = new HelperUITooltip()
                    {
                        Text = "Informe códigos GTIN (8, 12, 13, 14), de acordo com o NCM e CEST. Para produtos que não possuem código de barras, informe o literal “SEM GTIN”, se utilizar este produto para emitir notas fiscais."
                    }
                }
            };
        }
    }
}