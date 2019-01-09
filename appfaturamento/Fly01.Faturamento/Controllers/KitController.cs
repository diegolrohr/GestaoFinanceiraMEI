using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation.Commons;
using Fly01.uiJS.Enums;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoCadastrosKit)]
    public class KitController : BaseController<KitVM>
    {
        public override Func<KitVM, object> GetDisplayData()
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
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            formConfigKit.Elements.Add(new InputHiddenUI { Id = "id" });
            formConfigKit.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12", Label = "Descrição Kit", Required = true, MaxLength = 40 });

            #endregion Form fly01frmKit

            #region kit Itens

            string nomeCtrl = @"InventarioItem";
            var formConfigKitItens = new FormUI
            {
                Id = "fly01frmKitItens",
                ReadyFn = "fnFormReadyKitItens",
                UrlFunctions = Url.Action("Functions", nomeCtrl, null, Request.Url.Scheme) + "?fns=",
            };

            formConfigKitItens.Elements.Add(new InputHiddenUI { Id = "kitId", Name = "kitId" });

            formConfigKitItens.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "IdItens",
                Class = "col s8",
                Label = "Produto/ Serviço",
                Required = true,
                DataUrl = @Url.Action("Produto", "AutoComplete"),
                LabelId = "produto_servicos",
            }, ResourceHashConst.EstoqueCadastrosProdutos));

            formConfigKitItens.Elements.Add(new InputFloatUI
            {
                Id = "quantidadeItens",
                Class = "col s2",
                Label = "Quantidade",
            });

            formConfigKitItens.Elements.Add(new ButtonUI
            {
                Id = "btnAdicionar",
                Class = "col s5 m2",
                Value = "+",
                DomEvents = new List<DomEventUI>() {
                    new DomEventUI() { DomEvent = "click", Function = "fnAdicionaProduto" }
                }
            });

            #endregion


            #region DataTable Produtos / Serviços

            var dtConfig = new DataTableUI
            {
                Id = "dtConfig",
                UrlGridLoad = Url.Action("GridLoadKitItem", "KitItem"),
                UrlFunctions = Url.Action("Functions", "kitItem", null, Request.Url.Scheme) + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter {Id = "id", Required = true }
                }
            };

            dtConfig.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1, Searchable = false, Orderable = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "tipo", DisplayName = "Quant.", Priority = 4, Searchable = false, Orderable = false });

            #endregion


            cfg.Content.Add(formConfigKit);
            cfg.Content.Add(formConfigKitItens);
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
                    Title = "Kits",
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
    }
}