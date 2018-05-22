using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.ViewModel;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation.Commons;
using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Faturamento.Controllers
{
    public class SerieNotaFiscalInutilizadaController : BaseController<SerieNotaFiscalInutilizadaVM>
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();

            customFilters.AddParam("$filter", $"statusSerieNotaFiscal eq {AppDefaults.APIEnumResourceName}StatusSerieNotaFiscal'Inutilizada'");
            customFilters.AddParam("$select", "id,serie,tipoOperacaoSerieNotaFiscal,numNotaFiscal,dataInclusao");

            return customFilters;
        }

        public override Func<SerieNotaFiscalInutilizadaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                serie = x.Serie.PadLeft(3, '0'),
                tipoOperacaoSerieNotaFiscal = EnumHelper.GetValue(typeof(TipoOperacaoSerieNotaFiscal), x.TipoOperacaoSerieNotaFiscal),
                numNotaFiscal = x.NumNotaFiscal.ToString().PadLeft(8, '0'),
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index", "SerieNotaFiscalInutilizada") },
                Header = new HtmlUIHeader
                {
                    Title = "Notas Fiscais Inutilizadas",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Inutilizar Nota Fiscal", OnClickFn = "fnNovo" },
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad", "SerieNotaFiscalInutilizada"), UrlFunctions = Url.Action("Functions", "SerieNotaFiscalInutilizada", null, Request.Url.Scheme) + "?fns=" };

            config.Columns.Add(new DataTableUIColumn { DataField = "serie", DisplayName = "Série", Priority = 1 });

            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoOperacaoSerieNotaFiscal",
                DisplayName = "Operação da Série NF",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoOperacaoSerieNotaFiscal)))
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "numNotaFiscal", DisplayName = "Número da Nota Fiscal", Priority = 2 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create", "SerieNotaFiscalInutilizada"),
                    WithParams = Url.Action("Edit", "SerieNotaFiscalInutilizada")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Inutilizar Nota Fiscal",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton { Id = "save", Label = "Inutilizar", OnClickFn = "fnSalvarConfirmacao", Type = "submit" }
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
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoOperacaoSerieNotaFiscal)))
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "numNotaFiscal",
                Class = "col s12 m4",
                Label = "Número da Nota Fiscal",
                Required = true,
                MaxLength = 8,
                Data = new { inputmask = "'regex': '[0-9]*'" }
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }
    }
}