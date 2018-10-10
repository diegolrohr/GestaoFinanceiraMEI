using Fly01.Core;
using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
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

namespace Fly01.OrdemServico.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.OrdemServicoCadastroProdutos)]
    public class ProdutoController : BaseController<ProdutoVM>
    {
        protected Func<ProdutoVM, object> GetDisplayDataSelect { get; set; }
        protected string SelectProperties { get; set; }
        private string GrupoProdutoResourceHash { get; set; }

        public ProdutoController()
        {
            ExpandProperties = "grupoProduto($select=id,descricao),unidadeMedida($select=id,descricao)";
            SelectProperties = "id,codigoProduto,descricao,grupoProdutoId,tipoProduto,registroFixo,objetoDeManutencao";
            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoProduto = x.CodigoProduto,
                descricao = x.Descricao,
                tipoProduto = EnumHelper.GetValue(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoCSS = EnumHelper.GetCSS(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoDescricao = EnumHelper.GetDescription(typeof(TipoProduto), x.TipoProduto),
                objetoDeManutencao = x.ObjetoDeManutencao,
                manutencao_sn = x.ObjetoDeManutencao ? "Sim" : "Não",
                SimNaoCss = EnumHelper.GetCSS(typeof(BoolSimNao), x.ObjetoDeManutencao ? "Sim" : "Nao"),
                registroFixo = x.RegistroFixo,
            };
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$expand", ExpandProperties);
            customFilters.AddParam("$select", SelectProperties);

            return customFilters;
        }

        public override Func<ProdutoVM, object> GetDisplayData() => GetDisplayDataSelect;

        [OperationRole(NotApply = true)]
        public JsonResult GridLoadPos(Dictionary<string, string> filters = null)
        {
            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoProduto = x.CodigoProduto,
                descricao = x.Descricao,
                unidadeMedidaId = x.UnidadeMedidaId,
                valorVenda = x.ValorVenda.ToString("C", AppDefaults.CultureInfoDefault),
                saldoProduto = x.SaldoProduto,
                vendaTotal = Convert.ToDouble(x.ValorVenda * x.SaldoProduto).ToString("C", AppDefaults.CultureInfoDefault),
                registroFixo = x.RegistroFixo,
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

            AddElements(config.Elements, config.Helpers, null);

            cfg.Content.Add(config);

            return cfg;
        }

        private void AddElements(List<BaseUI> elements, List<HelperUI> helpers, bool? objetoDeManutencao)
        {
            elements.Add(new InputHiddenUI { Id = "id" });
            elements.Add(new InputHiddenUI { Id = "saldoProduto", Value = "0" });

            elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m9", Label = "Descrição", Required = true });
            elements.Add(new InputTextUI { Id = "codigoProduto", Class = "col s12 m3", Label = "Código" });
            elements.Add(new InputHiddenUI { Id = "tipoProduto", Value = "2" });
            elements.Add(new InputHiddenUI { Id = "registroFixo" });

            elements.Add(new AutoCompleteUI
            {
                Id = "unidadeMedidaId",
                Class = "col s12 m3",
                Label = "Unidade de medida",
                Required = true,
                DataUrl = @Url.Action("UnidadeMedida", "AutoComplete"),
                LabelId = "unidadeMedidaDescricao"
            });

            elements.Add(new InputCurrencyUI { Id = "valorVenda", Class = "col s12 m3", Label = "Valor Venda" });

            if (objetoDeManutencao.HasValue)
                elements.Add(new InputHiddenUI { Id = "objetoDeManutencao", Value = objetoDeManutencao.Value.ToString() });
            else
                elements.Add(new InputCheckboxUI
                {
                    Id = "objetoDeManutencao",
                    Class = "col s12 m6 l3",
                    Label = "Objeto de Manutenção",
                });

            elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });
        }

        public ContentResult FormModalProduto() => FormModal(false, "Adicionar produto");
        public ContentResult FormModalObjetoDeManutencao() => FormModal(true, "Adicionar Objeto de manutenção");

        private ContentResult FormModal(bool objetoDeManutencao, string title)
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = title,
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmProduto",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReadyModal"
            };

            AddElements(config.Elements, config.Helpers, objetoDeManutencao);

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}