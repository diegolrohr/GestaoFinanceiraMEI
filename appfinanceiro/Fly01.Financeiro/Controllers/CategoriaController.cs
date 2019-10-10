using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
    public class CategoriaController : BaseController<CategoriaVM>
    {
        public CategoriaController()
        {
            ExpandProperties = "categoriaPai($select=descricao)";
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });            
            target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });

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
                    Title = "Categoria",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frm",
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
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m9", Label = "Descrição", Required = true, MaxLength = 100 });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoCarteira",
                Class = "col s12 m3 l3",
                Label = "Entrada/Saida",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCarteira))),
                DomEvents = new List<DomEventUI>() { new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoCarteira" } }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "categoriaPaiId",
                Class = "col s12",
                Label = "Categoria Pai",

                DataUrl = Url.Action("CategoriaPai", "AutoComplete"),
                DataUrlPost = Url.Action("NovaCategoria"),
                LabelId = "categoriaPaiDescricao",
                PreFilter = "tipoCarteira"
            });

            cfg.Content.Add(config);
            return cfg;
        }

        public override Func<CategoriaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                descricao = x.Descricao,
                categoriaPaiId = x.CategoriaPaiId,
                tipoCarteira = x.TipoCarteira
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Categorias",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "Categoria", null, Request.Url?.Scheme) + "?fns="
            };

            var config = new DataTableUI
            {
                Id = "fly01dt",
                RowGroup = "tipoCarteira",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions", "Categoria", null, Request.Url?.Scheme) + "?fns=",
                Options = new DataTableUIConfig { PageLength = 50, NoExportButtons = true }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "descricao",
                DisplayName = string.Empty,
                Priority = 1,
                RenderFn = "fnRenderGroup",
                Searchable = false,
                Orderable = false
            });

            cfg.Content.Add(config);
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        [HttpPost]
        public JsonResult NovaCategoria(string term)
        {
            try
            {
                var entity = new CategoriaVM
                {
                    Descricao = term,
                    TipoCarteira = Request.QueryString["tipo"]
                };

                var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));
                var data = RestHelper.ExecutePostRequest<CategoriaVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }
    }
}
