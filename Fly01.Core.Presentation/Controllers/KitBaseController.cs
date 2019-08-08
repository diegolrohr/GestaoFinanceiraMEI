using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class KitBaseController<T> : BaseController<T> where T : KitVM
    {
        public override Func<T, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                descricao = x.Descricao,
            };
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Voltar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });
            }

            return target;
        }

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create"),
                    WithParams = Url.Action("Edit")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Cadastro de Kit",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader()),

                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            #region Form fly01frmKit

            var formConfigKit = new FormUI
            {
                Id = "fly01frmKit",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = Url.Action("List")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnDrawCallbackCalcProdutos" }
            };

            formConfigKit.Elements.Add(new InputHiddenUI { Id = "id" });
            formConfigKit.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12", Label = "Descrição", Required = true, MaxLength = 40 });

            #endregion Form fly01frmKit

            #region kit Itens

            var formConfigKitItens = new FormUI
            {
                Id = "fly01frmKitItens",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create", "KitItem")
                },
                ReadyFn = "fnFormReadyKitItens",
                UrlFunctions = Url.Action("Functions", "KitItem", null, Request.Url.Scheme) + "?fns=",
            };

            formConfigKitItens.Elements.Add(new InputHiddenUI { Id = "kitId" });
            formConfigKitItens.Elements.Add(new InputHiddenUI { Id = "tipoItem" });
            formConfigKitItens.Elements.Add(new InputHiddenUI { Id = "produtoId" });
            formConfigKitItens.Elements.Add(new InputHiddenUI { Id = "servicoId" });

            formConfigKitItens.Elements.Add(new LabelSetUI { Label = "Adicionar Produtos e Serviços" });

            formConfigKitItens.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "produtoServicoId",
                Class = "col s12 m9",
                Label = "Produto/ Serviço",
                Required = true,
                DataUrl = @Url.Action("ProdutoServico", "AutoComplete"),
                LabelId = "produtoServicoDescricao",
                PreFilter = "kitId",
                DomEvents = new List<DomEventUI>() {
                    new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnProdutoServicoSelected" }
                }
            }, ResourceHashConst.FaturamentoCadastrosKit));

            formConfigKitItens.Elements.Add(new InputFloatUI
            {
                Id = "quantidade",
                Class = "col s8 m2",
                Label = "Quantidade",
                Value = "1",
                Required = true
            });

            formConfigKitItens.Elements.Add(new ButtonUI
            {
                Id = "btnAdicionar",
                Class = "col s4 m1",
                Value = "+",
                DomEvents = new List<DomEventUI>() {
                    new DomEventUI() { DomEvent = "click", Function = "fnAdicionarItem" }
                }
            });
            formConfigKitItens.Elements.Add(new DivElementUI { Id = "divKitItensGrid", Class = "col s12 visible" });
            #endregion

            #region DataTable Produtos / Serviços

            var dtConfig = new DataTableUI
            {
                Id = "dtKitItens",
                UrlGridLoad = Url.Action("GridLoadKitItem", "KitItem"),
                UrlFunctions = Url.Action("Functions", "kitItem", null, Request.Url.Scheme) + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter {Id = "id", Required = true }
                },
                Functions = new List<string>() { "fnRenderEnum" },
                Parent = "divKitItensGridField",
                Options = new DataTableUIConfig
                {
                    OrderColumn = 0,
                    OrderDir = "asc"
                },
                Callbacks = new DataTableUICallbacks()
                {
                    DrawCallback = "fnDrawCallbackCalcProdutos"
                },
            };

            dtConfig.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnModalKitItem", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "produtoServicoCodigo", DisplayName = "Código", Priority = 4, Searchable = false, Orderable = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "produtoServicoDescricao", DisplayName = "Descrição", Priority = 1, Searchable = false, Orderable = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "quantidade", DisplayName = "Quantidade", Priority = 2, Searchable = false, Orderable = false });
            dtConfig.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoItem",
                DisplayName = "Tipo",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoItem))),
                RenderFn = "fnRenderEnum(full.tipoItemCssClass, full.tipoItemDescription)",
                Searchable = false,
                Orderable = false
            });

            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorVenda", DisplayName = "Valor Venda", Priority = 5, Searchable = false, Orderable = false});
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorCusto", DisplayName = "Valor custo", Priority = 6, Searchable = false, Orderable = false});
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorServico", DisplayName = "Valor Serviço", Priority = 7, Searchable = false, Orderable = false});

            #endregion

            #region Helpers
            formConfigKit.Helpers.Add(new TooltipUI
            {
                Id = "produtoServicoId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Pesquise produtos por: descrição, código ou código de barras. Pesquise serviços por: descrição ou código. Produtos/Serviços já adicionados ao kit não são listados."
                }
            });
            #endregion

            cfg.Content.Add(formConfigKit);
            cfg.Content.Add(formConfigKitItens);

            cfg.Content.Add(new DivUI
            {
                Elements = new List<BaseUI>
                {
                    new InputCurrencyUI { Id = "valorVendaTotal", Class = "col s12 m4", Label = "Total Venda Produto", Readonly = true },
                    new InputCurrencyUI { Id = "valorCustoTotal", Class = "col s12 m4", Label = "Total Custo Produto", Readonly = true },
                    new InputCurrencyUI { Id = "valorServicoTotal", Class = "col s12 m4", Label = "Total Serviço", Readonly = true },
                }
            });

            cfg.Content.Add(dtConfig);

            return cfg;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Kits Produtos/Serviços",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { Id = "fly01dt", UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}