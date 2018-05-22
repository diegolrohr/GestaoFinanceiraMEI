using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Compras.Controllers.Base;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using Fly01.Core;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Rest;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Compras.Controllers
{
    public class CategoriaController : BaseController<CategoriaVM>
    {
        public CategoriaController()
        {
            ExpandProperties = "categoriaPai($select=descricao)";
        }

        public override ContentResult Form()
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
                    Title = "Cadastro de Categoria",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
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
                    List = Url.Action("List")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReady"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m9", Label = "Descrição", Required = true, MaxLength = 40 });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoCarteira",
                Class = "col s12 m3",
                Label = "Entrada/Saida",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCarteira))),
                DomEvents = new List<DomEventUI>() { new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoCarteira" } }
            });

            config.Elements.Add(new AutocompleteUI
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
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override Func<CategoriaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                descricao = x.Descricao,
                categoriaPaiId = x.CategoriaPaiId,
                tipoCarteira = x.TipoCarteira,
                registroFixo = x.RegistroFixo
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Categoria",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "Categoria", null, Request.Url?.Scheme) + "?fns="
            };

            var config = new DataTableUI
            {
                RowGroup = "tipoCarteira",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions", "Categoria", null, Request.Url?.Scheme) + "?fns=",
                Options = new DataTableUIConfig { PageLength = 50 }
            };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" });
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

        #region OnDemmand

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

        #endregion
    }
}