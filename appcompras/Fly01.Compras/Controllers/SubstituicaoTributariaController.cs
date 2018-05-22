using Fly01.Compras.Controllers.Base;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    public class SubstituicaoTributariaController : BaseController<SubstituicaoTributariaVM>
    {
        protected Func<SubstituicaoTributariaVM, object> GetDisplayDataSelect { get; set; }
        protected string SelectProperties { get; set; }
        protected string OrderProperties { get; set; }

        public SubstituicaoTributariaController()
        {
            ExpandProperties = "ncm($select=id,descricao,codigo),estadoOrigem($select=id,nome,sigla),estadoDestino($select=id,nome,sigla),cest($select=id,descricao,codigo)";

            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                ncmId = x.NcmId,
                cestId = x.CestId,
                mva = x.Mva,
                estadoOrigemId = x.EstadoOrigemId,
                estadoDestinoId = x.EstadoDestinoId,
                ncm_codigo = x.Ncm != null ? x.Ncm.Codigo : "",
                cest_codigo = x.Cest != null ? x.Cest.Codigo : "",
                estadoOrigem_nome = x.EstadoOrigem != null ? x.EstadoOrigem.Nome : "",
                estadoDestino_nome = x.EstadoDestino != null ? x.EstadoDestino.Nome : "",
                tipoSubstituicaoTributaria = EnumHelper.GetValue(typeof(TipoSubstituicaoTributaria), x.TipoSubstituicaoTributaria),
                tipoSubstituicaoTributariaCSS = EnumHelper.GetCSS(typeof(TipoSubstituicaoTributaria), x.TipoSubstituicaoTributaria),
                tipoSubstituicaoTributariaDescricao = EnumHelper.GetDescription(typeof(TipoSubstituicaoTributaria), x.TipoSubstituicaoTributaria),
                registroFixo = x.RegistroFixo
            };
        }

        public override Func<SubstituicaoTributariaVM, object> GetDisplayData()
        {
            return GetDisplayDataSelect;
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$expand", ExpandProperties);

            return customFilters;
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
                    Title = "Dados da Substituição Tributária",
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
                    List = @Url.Action("List")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReady"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoSubstituicaoTributaria",
                Class = "col l3 m3 s12",
                Label = "Tipo",
                Value = "1",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoSubstituicaoTributaria)))
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "ncmId",
                Class = "col l9 m9 s12",
                Label = "NCM",
                Required = true,
                DataUrl = @Url.Action("Ncm", "AutoComplete"),
                LabelId = "ncmDescricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeNCM" } }
            });
            ;
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "cestId",
                Class = "col l12 m12 s12",
                Label = "CEST (Escolha um NCM antes)",
                DataUrl = @Url.Action("Cest", "AutoComplete"),
                LabelId = "cestDescricao",
                PreFilter = "ncmId"
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "mva",
                Class = "col l4 m4 s12",
                Label = "MVA",
                Required = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'numeric', 'suffix': ' %', 'autoUnmask': true, 'radixPoint': ',' " }
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "fcp",
                Class = "col m2 s12",
                Label = "FCP",
                Value = "0",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'numeric', 'suffix': ' %', 'autoUnmask': true, 'radixPoint': ',' " }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "estadoOrigemId",
                Class = "col l4 m4 s12",
                Label = "Origem",
                Required = true,
                DataUrl = @Url.Action("Estado", "AutoComplete"),
                LabelId = "estadoOrigemNome",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect" } }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "estadoDestinoId",
                Class = "col l4 m4 s12",
                Label = "Destino",
                Required = true,
                DataUrl = @Url.Action("Estado", "AutoComplete"),
                LabelId = "estadoDestinoNome",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect" } }
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Substituição Tributária",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" });

            config.Columns.Add(new DataTableUIColumn { DataField = "ncm_codigo", DisplayName = "NCM", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "cest_codigo", DisplayName = "CEST", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "mva", DisplayName = "MVA", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn { DataField = "estadoOrigem_nome", DisplayName = "Origem", Priority = 4 });
            config.Columns.Add(new DataTableUIColumn { DataField = "estadoDestino_nome", DisplayName = "Destino", Priority = 5 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoSubstituicaoTributaria",
                DisplayName = "Tipo",
                Priority = 6,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoSubstituicaoTributaria))),
                RenderFn = "function(data, type, full, meta) { return fnRenderEnum(full.tipoSubstituicaoTributariaCSS, full.tipoSubstituicaoTributariaDescricao); }",
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }
    }
}