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
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
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

            var tabs = new TabsUI
            {
                Id = "fly01tabs",
                Tabs = new List<TabsUIItem>()
                {
                    new TabsUIItem()
                    {
                        Id = "fly01frmKitProduto",
                        Title = "Produtos"
                    },
                    new TabsUIItem()
                    {
                        Id = "fly01frmKitServico",
                        Title = "Serviços"
                    }
                }
            };

            //formConfigInventario.Elements.Add(new InputHiddenUI { Id = "inventarioStatus" });
            formConfigKit.Elements.Add(new InputHiddenUI { Id = "id" });
            formConfigKit.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12", Label = "Descrição Kit", Required = true, MaxLength = 40 });

            #endregion Form fly01frmKit

            #region Form fly01 Produto /Serviço

            var formConfigKitProduto = new FormUI
            {
                Id = "fly01frmKitProduto",
                //ReadyFn = "fnFormReadyKitProduto",
                UrlFunctions = Url.Action("Functions", "KitItem", null, Request.Url.Scheme) + "?fns=",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create", "KitItem"),
                    Get = @Url.Action("Json", "KitItem") + "/"
                },
            };

            //formConfigKitProduto.Elements.Add(new LabelSetUI { Id = "kitItemLabelSet", Class = "col s12", Label = "Produtos" });

            formConfigKitProduto.Elements.Add(new InputHiddenUI { Id = "kitId", Name = "kitIdProduto" });

            formConfigKitProduto.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "produtoId",
                Class = "col s8",
                Label = "Produto/Código",
                Required = true,
                DataUrl = @Url.Action("Produto", "AutoComplete"),
                LabelId = "produtoDescricao",
            }, ResourceHashConst.EstoqueCadastrosProdutos));

            formConfigKitProduto.Elements.Add(new InputFloatUI
            {
                Id = "quantidadeProduto",
                Class = "col s2",
                Label = "Quantidade",
            });

            formConfigKitProduto.Elements.Add(new ButtonUI
            {
                Id = "btnAdicionar",
                Class = "col s5 m2",
                Value = "+",
                DomEvents = new List<DomEventUI>() {
                    new DomEventUI() { DomEvent = "click", Function = "fnAdicionaProduto" }
                }
            });
            formConfigKitProduto.Elements.Add(new DivElementUI { Id = "kitProdutos", Class = "col s12 visible" });

            var formConfigKitServico = new FormUI
            {
                Id = "fly01frmKitServico",
                //ReadyFn = "fnFormReadyKitServico",
                UrlFunctions = Url.Action("Functions", "KitItem", null, Request.Url.Scheme) + "?fns="
            };

            formConfigKitServico.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "servicoId",
                Class = "col s8",
                Label = "Serviço/Código",
                Required = true,
                DataUrl = @Url.Action("Servico", "AutoComplete"),
                LabelId = "servicoDescricao",
            }, ResourceHashConst.EstoqueCadastrosProdutos));

            formConfigKitServico.Elements.Add(new InputFloatUI
            {
                Id = "quantidadeServico",
                Class = "col s2",
                Label = "Quantidade",
            });

            formConfigKitServico.Elements.Add(new ButtonUI
            {
                Id = "btnAdicionarServico",
                Class = "col s2",
                Value = "+",
                DomEvents = new List<DomEventUI>() {
                    //new DomEventUI() { DomEvent = "click", Function = "fnAdicionaServico" }
                }
            });
            formConfigKitServico.Elements.Add(new DivElementUI { Id = "kitServicos", Class = "col s12 visible" });

            #endregion


            #region DataTable Produtos

            var dtConfig = new DataTableUI
            {
                Parent = "kitProdutosField",
                Id = "dtKitProdutos",
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
            #region DataTable Servicos

            var dtConfigServicos = new DataTableUI
            {
                Parent = "kitServicosField",
                Id = "dtKitServicos",
                UrlGridLoad = Url.Action("GridLoadKitItem", "KitItem"),
                UrlFunctions = Url.Action("Functions", "kitItem", null, Request.Url.Scheme) + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter {Id = "id", Required = true }
                }
            };

            dtConfigServicos.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            dtConfigServicos.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1, Searchable = false, Orderable = false });
            dtConfigServicos.Columns.Add(new DataTableUIColumn { DataField = "tipo", DisplayName = "Quant.", Priority = 4, Searchable = false, Orderable = false });

            #endregion

            cfg.Content.Add(formConfigKit);
            cfg.Content.Add(tabs);
            cfg.Content.Add(formConfigKitProduto);
            cfg.Content.Add(dtConfig);
            cfg.Content.Add(formConfigKitServico);
            cfg.Content.Add(dtConfigServicos);

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