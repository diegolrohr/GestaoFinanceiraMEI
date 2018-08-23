using Fly01.Core;
using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.OrdemServico.Enum;
using Fly01.OrdemServico.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.EstoqueCadastrosProdutos)]
    public class ProdutoController : ProdutoBaseController<ProdutoVM>
    {
        public ProdutoController()
            : base(ResourceHashConst.EstoqueCadastrosGrupoProdutos)
        {
            SelectProperties = "id,codigoProduto,descricao,grupoProdutoId,tipoProduto,registroFixo,objetoDeManutencao";
            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoProduto = x.CodigoProduto,
                descricao = x.Descricao,
                grupoProdutoId = x.GrupoProdutoId,
                grupoProduto_descricao = x.GrupoProduto != null ? x.GrupoProduto.Descricao : "",
                tipoProduto = EnumHelper.GetValue(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoCSS = EnumHelper.GetCSS(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoDescricao = EnumHelper.GetDescription(typeof(TipoProduto), x.TipoProduto),
                objetoDeManutencao = x.ObjetoDeManutencao,
                manutencao_sn = x.ObjetoDeManutencao ? "Sim" : "Não",
                registroFixo = x.RegistroFixo,
                SimNaoCss = EnumHelper.GetCSS(typeof(BoolSimNao), x.ObjetoDeManutencao ? "Sim" : "Nao"),
            };
        }

        public override Func<ProdutoVM, object> GetDisplayData()
        {
            return base.GetDisplayData();
        }

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

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Produtos",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "codigoProduto", DisplayName = "Código", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "manutencao_sn",
                DisplayName = "Objeto de Manutenção",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(BoolSimNao))),
                RenderFn = "fnRenderEnum(full.SimNaoCss, full.manutencao_sn)"
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create"),
                    WithParams = Url.Action("Edit")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Dados do Produto",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = Url.Action("List"),
                    Form = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReady"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "saldoProduto", Value = "0" });

            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m9", Label = "Descrição", Required = true });
            config.Elements.Add(new InputTextUI { Id = "codigoProduto", Class = "col s12 m3", Label = "Código" });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "unidadeMedidaId",
                Class = "col s12 m3",
                Label = "Unidade de medida",
                Required = true,
                DataUrl = @Url.Action("UnidadeMedida", "AutoComplete"),
                LabelId = "unidadeMedidaDescricao"
            });

            config.Elements.Add(new InputCurrencyUI { Id = "valorVenda", Class = "col s12 m3", Label = "Valor Venda" });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "objetoDeManutencao",
                Class = "col s12 m6 l3",
                Label = "Objeto de Manutenção",
            });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });

            List<TooltipUI> tooltips = GetHelpers();

            if (tooltips != null)
                config.Helpers.AddRange(tooltips);

            cfg.Content.Add(config);

            return cfg;
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