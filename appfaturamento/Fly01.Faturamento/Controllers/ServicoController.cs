using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.ViewModel;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core.Presentation.Commons;

namespace Fly01.Faturamento.Controllers
{
    public class ServicoController : BaseController<ServicoVM>
    {
        protected Func<ServicoVM, object> GetDisplayDataSelect { get; set; }
        protected string SelectProperties { get; set; }

        public ServicoController()
        {
            ExpandProperties = "nbs($select=id,descricao)";
            //SelectProperties = "id,codigoServico,descricao,tipoServico";
            SelectProperties = "id,codigoServico,descricao";

            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoServico = x.CodigoServico,
                descricao = x.Descricao,
                //tipoServico = EnumHelper.SubtitleDataAnotation("TipoServico", x.TipoServico).Value,
                //tipoServicoCSS = EnumHelper.SubtitleDataAnotation("TipoServico", x.TipoServico).CssClass,
                //tipoServicoDescricao = EnumHelper.SubtitleDataAnotation("TipoServico", x.TipoServico).Description
            };
        }

        public override Func<ServicoVM, object> GetDisplayData()
        {
            return GetDisplayDataSelect;
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$expand", ExpandProperties);
            customFilters.AddParam("$select", SelectProperties);

            return customFilters;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Serviços",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" });

            config.Columns.Add(new DataTableUIColumn { DataField = "codigoServico", DisplayName = "Código", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });
            //config.Columns.Add(new DataTableUIColumn
            //{
            //    DataField = "tipoServico",
            //    DisplayName = "Tipo",
            //    Priority = 4,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoServico", true, false)),
            //    RenderFn = "function(data, type, full, meta) { return \"<span class=\\\"new badge \" + full.tipoServicoCSS + \" left\\\" data-badge-caption=\\\" \\\">\" + full.tipoServicoDescricao + \"</span>\" }"
            //});

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
                    Title = "Dados do Serviço",
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
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputTextUI { Id = "codigoServico", Class = "col l3 m3 s12", Label = "Código", Required = true });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col l9 m9 s12", Label = "Descrição", Required = true });

            //config.Elements.Add(new SelectUI { Id = "tipoServico", Class = "col l3 m3 s12", Label = "Tipo",Required = true)
            //{
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoServico", true, false))
            //});
            config.Elements.Add(new AutocompleteUI
            {
                Id = "nbsId",
                Class = "col s12",
                Label = "NBS",
                DataUrl = @Url.Action("Nbs", "AutoComplete"),
                LabelId = "nbsDescricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect" } }
            });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoPagamentoImpostoISS",
                Class = "col l3 m3 s12",
                Label = "Tipo Pagto ISS",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoPagamentoImpostoISS", true, false))
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoISS",
                Class = "col l6 m6 s12",
                Label = "Tipo Tributação ISS",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoTributacaoISS", true, false))
            });

            config.Elements.Add(new InputCurrencyUI { Id = "valorServico", Class = "col l3 m3 s12", Label = "Valor Servico", Required = true });

            config.Elements.Add(new TextareaUI { Id = "observacao", Class = "col l12 m12 s12", Label = "Observação", MaxLength = 200 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Serviço",
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
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputTextUI { Id = "codigoServico", Class = "col l6 m6 s12", Label = "Código", Required = true });
            config.Elements.Add(new InputCurrencyUI { Id = "valorServico", Class = "col l6 m6 s12", Label = "Valor Servico", Required = true });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col l12 m12 s12", Label = "Descrição", Required = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}