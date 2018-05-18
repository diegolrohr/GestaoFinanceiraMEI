using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Estoque.Controllers.Base;
using Fly01.Estoque.ViewModel;
using Fly01.Core.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using Fly01.Core.API;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Estoque.Controllers
{
    public class TipoMovimentoController : BaseController<TipoMovimentoVM>
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$select", "id,descricao,tipoEntradaSaida,registroFixo");

            return customFilters;
        }

        public override Func<TipoMovimentoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                descricao = x.Descricao,
                tipoEntradaSaida = x.TipoEntradaSaida,
                tipoEntradaSaidaDescription = EnumHelper.GetDescription(typeof(TipoEntradaSaida), x.TipoEntradaSaida),
                tipoEntradaSaidaCssClass = EnumHelper.GetCSS(typeof(TipoEntradaSaida), x.TipoEntradaSaida),
                tipoEntradaSaidaValue = EnumHelper.GetValue(typeof(TipoEntradaSaida), x.TipoEntradaSaida),
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
                    Title = "Tipos de movimento",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "TipoMovimento", null, Request.Url?.Scheme) + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions", "TipoMovimento", null, Request.Url?.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" });

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoEntradaSaida",
                DisplayName = "Entrada/Saida",
                Priority = 2,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoEntradaSaida))),
                RenderFn = "function(data, type, row, meta) { return createElem(\"span\", {\"class\":\"new badge \" + row.tipoEntradaSaidaCssClass + \" left\", \"data-badge-caption\": \" \" }, row.tipoEntradaSaidaValue).outerHTML; }"
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
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
                    Title = "Dados do tipo de movimento",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "TipoMovimento", null, Request.Url?.Scheme) + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                UrlFunctions = Url.Action("Functions", "TipoMovimento", null, Request.Url?.Scheme) + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col l8 s12", Label = "Descricao", Required = true, MaxLength = 40 });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoEntradaSaida",
                Class = "col l3 s12",
                Label = "Entrada / Saída",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoEntradaSaida)))
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }
    }
}