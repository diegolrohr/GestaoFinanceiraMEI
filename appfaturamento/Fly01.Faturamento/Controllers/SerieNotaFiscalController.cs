﻿using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.Entities.ViewModel;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;

namespace Fly01.Faturamento.Controllers
{
    public class SerieNotaFiscalController : BaseController<SerieNotaFiscalVM>
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();

            customFilters.AddParam("$filter", "statusSerieNotaFiscal eq Fly01.Faturamento.Domain.Enums.StatusSerieNotaFiscal'Habilitada'");
            customFilters.AddParam("$select", "id,serie,tipoOperacaoSerieNotaFiscal,numNotaFiscal,dataInclusao");

            return customFilters;
        }

        public override Func<SerieNotaFiscalVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                serie = x.Serie.PadLeft(3, '0'),
                tipoOperacaoSerieNotaFiscal = EnumHelper.SubtitleDataAnotation("TipoOperacaoSerieNotaFiscal", x.TipoOperacaoSerieNotaFiscal).Value,
                numNotaFiscal = x.NumNotaFiscal.ToString().PadLeft(8, '0'),
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index", "SerieNotaFiscal") },
                Header = new HtmlUIHeader
                {
                    Title = "Série de Notas Fiscais",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Adicionar", OnClickFn = "fnNovo" },
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad", "SerieNotaFiscal"), UrlFunctions = Url.Action("Functions", "SerieNotaFiscal", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" });

            config.Columns.Add(new DataTableUIColumn { DataField = "serie", DisplayName = "Série", Priority = 1 });

            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoOperacaoSerieNotaFiscal",
                DisplayName = "Operação da Série NF",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoOperacaoSerieNotaFiscal", true, false))
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "numNotaFiscal", DisplayName = "Próxima NF Emitida", Priority = 2 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create", "SerieNotaFiscal"),
                    WithParams = Url.Action("Edit", "SerieNotaFiscal")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Dados da Série da Nota Fiscal",
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
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputHiddenUI { Id = "statusSerieNotaFiscal" });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "serie",
                Class = "col s12 m4",
                Label = "Série",
                Required = true,
                MaxLength = 3,
                Data = new { inputmask = "'regex': '[0-9]*'" }
            });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoOperacaoSerieNotaFiscal",
                Class = "col s12 m4",
                Label = "Tipo de Operação",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoOperacaoSerieNotaFiscal", true, false))
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "numNotaFiscal",
                Class = "col s12 m4",
                Label = "Próxima Nota a ser Emitida",
                Required = true,
                MaxLength = 8,
                Data = new { inputmask = "'regex': '[0-9]*'" }
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Série da Nota Fiscal",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmModalSerieNotaFiscal"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputHiddenUI { Id = "statusSerieNotaFiscal" });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "serie",
                Class = "col s12 m4",
                Label = "Série",
                Required = true,
                MaxLength = 3,
                Data = new { inputmask = "'regex': '[0-9]*'" }
            });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoOperacaoSerieNotaFiscal",
                Class = "col s12 m4",
                Label = "Tipo de Operação",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoOperacaoSerieNotaFiscal", true, false)),
                Value = "1"
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "numNotaFiscal",
                Class = "col s12 m4",
                Label = "Próxima Nota a ser Emitida",
                Required = true,
                MaxLength = 8,
                Data = new { inputmask = "'regex': '[0-9]*'" },
                Value = "1"
            });
            
            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Default), "application/json");
        }
        
    }
}