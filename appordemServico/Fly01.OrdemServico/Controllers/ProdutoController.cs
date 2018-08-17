using System;
using Fly01.Core;
using System.Web.Mvc;
using Fly01.Core.Helpers;
using System.Collections.Generic;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Enums;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.EstoqueCadastrosProdutos)]
    public class ProdutoController : ProdutoBaseController<ProdutoVM>
    {
        public ProdutoController()
            : base(ResourceHashConst.EstoqueCadastrosGrupoProdutos) { }

        [OperationRole(NotApply = true)]
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

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });
            }

            return target;
        }

        [OperationRole(NotApply = true)]
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

        [OperationRole(NotApply = true)]
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

        protected override ContentUI FormJson()
        {
            var result = base.FormJson();
            var elements = ((FormUI)result.Content[0]).Elements;
            var index = elements.FindIndex(x => x is TextAreaUI && x.Id == "observacao");

            elements.Insert(index, // Novo campo para ORDEM DE SERVIÇO
                new InputCheckboxUI
                {
                    Id = "ObjetoDeManutencao",
                    Class = "col s12 m6 l3",
                    Label = "Objeto de Manutenção",
                    DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });

            return result;
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