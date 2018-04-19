using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.Commons;
using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.Entities.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    public class GrupoTributarioController : BaseController<GrupoTributarioVM>
    {
        public GrupoTributarioController()
        {
            ExpandProperties = "cfop";
        }

        public override Func<GrupoTributarioVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                descricao = x.Descricao,
                cfopId = x.CfopId,
                cfop_descricao = string.IsNullOrEmpty(x.Cfop.Descricao) ? "" : x.Cfop.Descricao.Substring(0, x.Cfop.Descricao.Length <= 100 ? x.Cfop.Descricao.Length : 100),
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index", "GrupoTributario") },
                Header = new HtmlUIHeader
                {
                    Title = "Grupo Tributário",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad", "GrupoTributario"), UrlFunctions = Url.Action("Functions", "GrupoTributario", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" });

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "cfop_descricao", DisplayName = "CFOP", Priority = 2 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create", "GrupoTributario"),
                    WithParams = Url.Action("Edit", "GrupoTributario")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Cadastro de Grupo Tributário",
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
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 l6", Label = "Descrição", Required = true, MaxLength = 40 });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "cfopId",
                Class = "col s12 l6",
                Label = "Código Fiscal de Operação (CFOP)",
                Required = true,
                DataUrl = @Url.Action("Cfop", "AutoComplete"),
                LabelId = "cfopDescricao"
            });

            //ST
            config.Elements.Add(new LabelsetUI { Id = "stLabel", Class = "col s12", Label = "Substituição Tributária" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaSubstituicaoTributaria",
                Class = "col s12 l12",
                Label = "Calcula Substituição Tributária",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowST" }
                }
            });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaIpiBaseST", Class = "col s12 m4", Label = "Aplica valor do IPI na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaFreteBaseST", Class = "col s12 m4", Label = "Aplica FRETE na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaDespesaBaseST", Class = "col s12 m4", Label = "Aplica DESPESAS na base de cálculo", Disabled = true });


            //ICMS
            config.Elements.Add(new LabelsetUI { Id = "simulatorLabel", Class = "col s12", Label = "ICMS" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaIcms",
                Class = "col s12 l12",
                Label = "Calcula ICMS",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowICMS" }
                }
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoICMS",
                Class = "col s12 l12",
                Label = "Situação da Operação no Simples Nacional",
                Disabled = true,
                ConstrainWidth = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoICMS)))
            });
            config.Elements.Add(new InputCheckboxUI { Id = "calculaIcmsDifal", Class = "col s12 m6 l6", Label = "Calcula ICMS DIFAL", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaIpiBaseIcms", Class = "col s12 m6 l6", Label = "Aplica valor do IPI na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaFreteBaseIcms", Class = "col s12 m6 l6", Label = "Aplica FRETE na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaDespesaBaseIcms", Class = "col s12 m6 l6", Label = "Aplica DESPESAS na base de cálculo", Disabled = true });

            //Aba IPI
            config.Elements.Add(new LabelsetUI { Id = "simulatorLabel", Class = "col s12", Label = "IPI" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaIpi",
                Class = "col s12 l12",
                Label = "Calcula IPI",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowIPI" }
                }
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoIPI",
                Class = "col s12 l12",
                Label = "Situação Tributária",
                Disabled = true,
                ConstrainWidth = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoIPI)).
                ToList().FindAll(x => "SaidaTributada,SaidaTributadaComAliquotaZero,SaidaIsenta,SaidaNaoTributada,SaidaImune,SaidaComSuspensao,OutrasSaidas".Contains(x.Value)))
            });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaFreteBaseIpi", Class = "col s12 m6 l6", Label = "Aplica FRETE na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaDespesaBaseIpi", Class = "col s12 m6 l6", Label = "Aplica DESPESAS na base de cálculo", Disabled = true });

            //PIS
            config.Elements.Add(new LabelsetUI { Id = "simulatorLabel", Class = "col s12", Label = "PIS" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaPis",
                Class = "col s12 l12",
                Label = "Calcula PIS",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowPIS" }
                }
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoPIS",
                Class = "col s12 l12",
                Label = "Situação Tributária",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoPISCOFINS))),
                ConstrainWidth = true
            });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaFreteBasePis", Class = "col s12 m6 l6", Label = "Aplica FRETE na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaDespesaBasePis", Class = "col s12 m6 l6", Label = "Aplica DESPESAS na base de cálculo", Disabled = true });

            //COFINS
            config.Elements.Add(new LabelsetUI { Id = "simulatorLabel", Class = "col s12", Label = "COFINS" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaCofins",
                Class = "col s12 l12",
                Label = "Calcula COFINS",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowCOFINS" }
                }
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoCOFINS",
                Class = "col s12 l12",
                Label = "Situação Tributária",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoPISCOFINS))),
                ConstrainWidth = true
            });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaFreteBaseCofins", Class = "col s12 m6 l6", Label = "Aplica FRETE na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaDespesaBaseCofins", Class = "col s12 m6 l6", Label = "Aplica DESPESAS na base de cálculo", Disabled = true });

            //ISS
            config.Elements.Add(new LabelsetUI { Id = "simulatorLabel", Class = "col s12", Label = "ISS" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaIss",
                Class = "col s12 l12",
                Label = "Calcula ISS",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowISS" }
                }
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoISS",
                Class = "col s12 l12",
                Label = "Situação Tributária",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoISS))),
                ConstrainWidth = true
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoPagamentoImpostoISS",
                Class = "col s12 l6",
                Label = "Pagamento de Imposto",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoPagamentoImpostoISS)))
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoCFPS",
                Class = "col s12 l6",
                Label = "Tipo CFPS",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCFPS)))
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Grupo Tributário",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmModalGrupoTributario"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaIcms", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaIpi", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaPis", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaCofins", Value = "false" });
            config.Elements.Add(new InputHiddenUI { Id = "calculaIss", Value = "false" });

            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 l12", Label = "Descrição", Required = true, MaxLength = 40 });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "cfopId",
                Class = "col s12 l12",
                Label = "Código Fiscal de Operação (CFOP)",
                Required = true,
                DataUrl = @Url.Action("Cfop", "AutoComplete"),
                LabelId = "cfopDescricao"
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
            {
                filters = new Dictionary<string, string>();
            }

            filters.Add("cfop/tipo eq ", $"{AppDefaults.APIEnumResourceName}TipoCfop'Saida'");

            return base.GridLoad(filters);
        }
    }
}