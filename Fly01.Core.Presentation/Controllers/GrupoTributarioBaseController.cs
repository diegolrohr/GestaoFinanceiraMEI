using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class GrupoTributarioBaseController<T> : BaseController<T> where T : GrupoTributarioVM
    {
        public GrupoTributarioBaseController()
        {
            ExpandProperties = "cfop($select=descricao)";
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryString = base.GetQueryStringDefaultGridLoad();
            queryString.Add("$select", "id,descricao,cfopId,registroFixo");
            return queryString;
        }

        public override Func<T, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                descricao = x.Descricao,
                cfopId = x.CfopId,
                cfop_descricao = string.IsNullOrEmpty(x.Cfop?.Descricao) ? "" : x.Cfop.Descricao.Substring(0, x.Cfop?.Descricao.Length <= 100 ? x.Cfop.Descricao.Length : 100),
                registroFixo = x.RegistroFixo
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index", "GrupoTributario") },
                Header = new HtmlUIHeader
                {
                    Title = "Grupo Tributário",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { Id = "fly01dt", UrlGridLoad = Url.Action("GridLoad", "GrupoTributario"), UrlFunctions = Url.Action("Functions", "GrupoTributario", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "cfop_descricao", DisplayName = "CFOP", Priority = 2 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Out });
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
                    Default = Url.Action("Create", "GrupoTributario"),
                    WithParams = Url.Action("Edit", "GrupoTributario")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Cadastro de Grupo Tributário",
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
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12", Label = "Descrição", Required = true, MaxLength = 60 });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "cfopId",
                Class = "col s12",
                Label = "Código Fiscal de Operação (CFOP)",
                DataUrl = @Url.Action("Cfop", "AutoComplete"),
                LabelId = "cfopDescricao"
            });

            #region Ambos
            config.Elements.Add(new DivElementUI { Id = "collapseAmbos", Class = "col s12 visible" });

            //PIS
            config.Elements.Add(new LabelSetUI { Id = "pisLabel", Class = "col s12", Label = "PIS" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaPis",
                Class = "col s12 m6 l6",
                Label = "Calcula PIS",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowPIS" }
                }
            });
            config.Elements.Add(new InputCheckboxUI { Id = "retemPis", Class = "col s12 m6 l6", Label = "Retém PIS", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoPIS",
                Class = "col s12 l12",
                Label = "Situação Tributária",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoPISCOFINS))),
                ConstrainWidth = true
            });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaFreteBasePis", Class = "col s12 m6 l6", Label = "Aplica FRETE na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaDespesaBasePis", Class = "col s12 m6 l6", Label = "Aplica DESPESAS na base de cálculo", Disabled = true });

            //COFINS
            config.Elements.Add(new LabelSetUI { Id = "cofinsLabel", Class = "col s12", Label = "COFINS" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaCofins",
                Class = "col s12 m6 l6",
                Label = "Calcula COFINS",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowCOFINS" }
                }
            });
            config.Elements.Add(new InputCheckboxUI { Id = "retemCofins", Class = "col s12 m6 l6", Label = "Retém COFINS", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoCOFINS",
                Class = "col s12 l12",
                Label = "Situação Tributária",
                Options = GetTipoTributacao(),
                ConstrainWidth = true
            });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaFreteBaseCofins", Class = "col s12 m6 l6", Label = "Aplica FRETE na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaDespesaBaseCofins", Class = "col s12 m6 l6", Label = "Aplica DESPESAS na base de cálculo", Disabled = true });
            #endregion

            #region NFe
            config.Elements.Add(new DivElementUI { Id = "collapseNFe", Class = "col s12 visible" });

            //ST
            config.Elements.Add(new LabelSetUI { Id = "stLabel", Class = "col s12", Label = "Substituição Tributária" });
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
            config.Elements.Add(new LabelSetUI { Id = "icmsLabel", Class = "col s12", Label = "ICMS" });
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
                ConstrainWidth = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoICMS)))
            });
            config.Elements.Add(new InputCheckboxUI { Id = "calculaIcmsDifal", Class = "col s12 m6 l6", Label = "Calcula ICMS DIFAL", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaIpiBaseIcms", Class = "col s12 m6 l6", Label = "Aplica valor do IPI na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaFreteBaseIcms", Class = "col s12 m6 l6", Label = "Aplica FRETE na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaDespesaBaseIcms", Class = "col s12 m6 l6", Label = "Aplica DESPESAS na base de cálculo", Disabled = true });

            //IPI
            config.Elements.Add(new LabelSetUI { Id = "ipiLabel", Class = "col s12", Label = "IPI" });
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
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoIPI)))
            });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaFreteBaseIpi", Class = "col s12 m6 l6", Label = "Aplica FRETE na base de cálculo", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "aplicaDespesaBaseIpi", Class = "col s12 m6 l6", Label = "Aplica DESPESAS na base de cálculo", Disabled = true });
            #endregion

            #region NFSe
            config.Elements.Add(new DivElementUI { Id = "collapseNFSe", Class = "col s12 visible" });
            //ISS
            config.Elements.Add(new LabelSetUI { Id = "issLabel", Class = "col s12", Label = "ISS" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaIss",
                Class = "col s12 m6 l6",
                Label = "Calcula ISS",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowISS" }
                }
            });
            config.Elements.Add(new InputCheckboxUI { Id = "retemISS", Class = "col s12 m6 l6", Label = "Retém ISS", Disabled = true });

            config.Elements.Add(new InputHiddenUI() { Id = "tipoTributacaoISS", Value = "T00" });
            config.Elements.Add(new InputHiddenUI() { Id = "tipoPagamentoImpostoISS", Value = "DentroMunicipio" });
            config.Elements.Add(new InputHiddenUI() { Id = "tipoCFPS", Value = "Tomador" });

            //config.Elements.Add(new SelectUI ?? Jamal add, não usamos ainda
            //{
            //    Id = "tipoTributacaoISS",
            //    Class = "col s12 l12",
            //    Label = "Situação Tributária",
            //    Disabled = true,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoISS))),
            //    ConstrainWidth = true
            //});
            //config.Elements.Add(new SelectUI
            //{
            //    Id = "tipoPagamentoImpostoISS",
            //    Class = "col s12 l6",
            //    Label = "Pagamento de Imposto",
            //    Disabled = true,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoPagamentoImpostoISS)))
            //});
            //config.Elements.Add(new SelectUI
            //{
            //    Id = "tipoCFPS",
            //    Class = "col s12 l6",
            //    Label = "Tipo CFPS",
            //    Disabled = true,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCFPS)))
            //});

            //CSLL
            config.Elements.Add(new LabelSetUI { Id = "csllLabel", Class = "col s12", Label = "CSLL" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaCSLL",
                Class = "col s12 m6 l6",
                Label = "Calcula CSLL",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowCSLL" }
                }
            });
            config.Elements.Add(new InputCheckboxUI { Id = "retemCSLL", Class = "col s12 m6 l6", Label = "Retém CSLL", Disabled = true });

            //INSS
            config.Elements.Add(new LabelSetUI { Id = "inssLabel", Class = "col s12", Label = "INSS" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaINSS",
                Class = "col s12 m6 l6",
                Label = "Calcula INSS",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowINSS" }
                }
            });
            config.Elements.Add(new InputCheckboxUI { Id = "retemINSS", Class = "col s12 m6 l6", Label = "Retém INSS", Disabled = true });

            //Imposto de Renda
            config.Elements.Add(new LabelSetUI { Id = "impostoRendaLabel", Class = "col s12", Label = "Imposto de Renda" });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "calculaImpostoRenda",
                Class = "col s12 m6 l6",
                Label = "Calcula Imposto de Renda",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnShowImpostoRenda" }
                }
            });
            config.Elements.Add(new InputCheckboxUI { Id = "retemImpostoRenda", Class = "col s12 m6 l6", Label = "Retém Imposto de Renda", Disabled = true });
            #endregion

            #region Helpers
            config.Helpers.Add(new TooltipUI
            {
                Id = "calculaSubstituicaoTributaria",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Necessário estar cadastrado no menu de Substituição Tributária, de acordo com a situação do estado origem e destino."
                }
            });
            #endregion

            cfg.Content.Add(config);

            return cfg;
        }

        private static List<SelectOptionUI> GetTipoTributacao()
        {
            var queryString = new Dictionary<string, string>();
            var returnObj = RestHelper.ExecuteGetRequest<ParametroTributarioVM>("parametrotributario", queryString);

            if (returnObj.TipoCRT == "SimplesNacional")
                return new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoICMS)));
            else
                return new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoICMS)));
        }
    }
}