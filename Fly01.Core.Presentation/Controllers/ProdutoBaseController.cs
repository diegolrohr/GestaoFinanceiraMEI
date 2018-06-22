using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
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
    public class ProdutoBaseController<T> : BaseController<T> where T : ProdutoVM
    {
        protected Func<ProdutoVM, object> GetDisplayDataSelect { get; set; }
        protected string SelectProperties { get; set; }
        public ProdutoBaseController()
        {
            ExpandProperties = "grupoProduto($select=id,descricao),unidadeMedida($select=id,descricao),ncm($select=id,descricao),cest($select=id,descricao,codigo),enquadramentoLegalIPI($select=id,codigo,grupoCST,descricao)";

            SelectProperties = "id,codigoProduto,descricao,grupoProdutoId,tipoProduto,registroFixo";
            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoProduto = x.CodigoProduto,
                descricao = x.Descricao,
                grupoProdutoId = x.GrupoProdutoId,
                grupoProduto_descricao = x.GrupoProduto != null ? x.GrupoProduto.Descricao : "",
                tipoProduto = EnumHelper.GetValue(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoCSS = EnumHelper.GetCSS(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoDescricao = EnumHelper.GetDescription(typeof(TipoProduto), x.TipoProduto),
                registroFixo = x.RegistroFixo
            };
        }

        public override Func<T, object> GetDisplayData()
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
                    Title = "Produtos",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" });

            config.Columns.Add(new DataTableUIColumn { DataField = "codigoProduto", DisplayName = "Código", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "grupoProduto_descricao", DisplayName = "Grupo", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoProduto",
                DisplayName = "Tipo",
                Priority = 4,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto))),
                RenderFn = "function(data, type, full, meta) { return fnRenderEnum(full.tipoProdutoCSS, full.tipoProdutoDescricao); }"
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
                    Title = "Dados do Produto",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out },
                        new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Out },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main }
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
                    List = Url.Action("List"),
                    Form = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReady"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "saldoProduto", Value = "0" });

            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m9", Label = "Descrição", Required = true });
            config.Elements.Add(new InputTextUI { Id = "codigoProduto", Class = "col s12 m3", Label = "Código" });

            config.Elements.Add(new InputTextUI { Id = "codigoBarras", Class = "col s12 m3", Label = "Código de barras", Value = "SEM GTIN" });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoProduto",
                Class = "col s12 m3",
                Label = "Tipo",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto))),
                DomEvents = new List<DomEventUI>() { new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoProduto" } }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "grupoProdutoId",
                Class = "col s12 m3",
                Label = "Grupo",
                Required = true,
                DataUrl = @Url.Action("GrupoProduto", "AutoComplete"),
                DataUrlPost = @Url.Action("NovoGrupoProduto"),
                LabelId = "grupoProdutoDescricao",
                PreFilter = "tipoProduto",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeGrupoProduto" } }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "unidadeMedidaId",
                Class = "col s12 m3",
                Label = "Unidade de medida",
                Required = true,
                DataUrl = @Url.Action("UnidadeMedida", "AutoComplete"),
                LabelId = "unidadeMedidaDescricao"
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "ncmId",
                Class = "col s12 m9",
                Label = "NCM",
                DataUrl = @Url.Action("Ncm", "AutoComplete"),
                LabelId = "ncmDescricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeNCM" } }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaIpi",
                Class = "col s12 m3",
                Label = "Alíquota IPI",
                MaxLength = 5,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'numeric', 'suffix': ' %', 'autoUnmask': true, 'radixPoint': ',' " }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "cestId",
                Class = "col s12",
                Label = "CEST (Escolha um NCM antes)",
                DataUrl = @Url.Action("Cest", "AutoComplete"),
                LabelId = "cestDescricao",
                PreFilter = "ncmId"
            });

            config.Elements.Add(new AutoCompleteUI()
            {
                Id = "enquadramentoLegalIPIId",
                Class = "col s12",
                Label = "Enquadramento Legal do IPI",
                DataUrl = @Url.Action("EnquadramentoLegalIPI", "AutoComplete"),
                LabelId = "enquadramentoLegalIPIDescricao"
            });

            config.Elements.Add(new InputNumbersUI { Id = "saldoMinimo", Class = "col s12 m3", Label = "Saldo Mínimo" });
            config.Elements.Add(new InputNumbersUI
            {
                Id = "saldoProdutoField",
                Class = "col s12 m3",
                Label = "Saldo Atual",
                Value = "0",
                Disabled = true,
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "blur", Function = "fnChangeSaldoProduto" } }
            });
            config.Elements.Add(new InputCurrencyUI { Id = "valorCusto", Class = "col s12 m3", Label = "Valor Custo" });
            config.Elements.Add(new InputCurrencyUI { Id = "valorVenda", Class = "col s12 m3", Label = "Valor Venda" });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });

            List<TooltipUI> tooltips = GetHelpers();

            if (tooltips != null)
                config.Helpers.AddRange(tooltips);

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public virtual List<TooltipUI> GetHelpers()
        {
            return null;
            //config.Helpers.Add(new TooltipUI
            //{
            //    Id = "codigoBarras",
            //    Tooltip = new HelperUITooltip()
            //    {
            //        Text = "Informe códigos GTIN (8, 12, 13, 14), de acordo com o NCM e CEST. Para produtos que não possuem código de barras, informe o literal “SEM GTIN”, se utilizar este produto para emitir notas fiscais."
            //    }
            //});
        }

        #region onDemand

        [HttpPost]
        public JsonResult NovoGrupoProduto(string term)
        {
            try
            {
                var tipoProduto = Request.QueryString["tipo"];

                var entity = new GrupoProdutoVM
                {
                    Descricao = term,
                    TipoProduto = tipoProduto
                };

                var resourceName = AppDefaults.GetResourceName(typeof(GrupoProdutoVM));
                var data = RestHelper.ExecutePostRequest<GrupoProdutoVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }

        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar produto",
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
                ReadyFn = "fnFormReadyModal"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "valorCusto", Value = "0" });
            config.Elements.Add(new InputHiddenUI { Id = "valorVenda", Value = "0" });

            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m9", Label = "Descrição", Required = true });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoProduto",
                Class = "col s12 m3",
                Label = "Tipo",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto))),
                DomEvents = new List<DomEventUI>() { new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoProduto" } }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "grupoProdutoId",
                Class = "col s12 m7",
                Label = "Grupo",
                Required = true,
                DataUrl = @Url.Action("GrupoProduto", "AutoComplete"),
                DataUrlPost = @Url.Action("NovoGrupoProduto"),
                LabelId = "grupoProdutoDescricao",
                PreFilter = "tipoProduto",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeGrupoProduto" } }
            });

            config.Elements.Add(new InputNumbersUI
            {
                Id = "saldoProduto",
                Class = "col s12 m2",
                Label = "Saldo atual",
                Value = "0",
                Required = true
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "unidadeMedidaId",
                Class = "col s12 m3",
                Label = "Unidade de medida",
                Required = true,
                DataUrl = @Url.Action("UnidadeMedida", "AutoComplete"),
                LabelId = "unidadeMedidaDescricao"
            });


            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        #endregion
    }
}