using Fly01.Estoque.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core;
using Fly01.Core.Presentation.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.EstoqueEstoqueAjusteManual)]
    public class AjusteManualController : BaseController<AjusteManualVM>
    {
        public override Func<AjusteManualVM, object> GetDisplayData() { throw new NotImplementedException(); }

        public override ContentResult List()
            => Form();

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public override ContentResult Form() => base.Form();

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
                    Title = "Ajuste manual",
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
                    List = @Url.Action("Form")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>
                {
                    "fnEntradaSaida",
                    "fnChangeTipoProduto",
                    "fnChangeGrupoProduto",
                    "fnChangeNCM"
                }
            };

            config.Elements.Add(new SelectUI()
            {
                Id = "tipoEntradaSaida",
                Class = "col l6 m6 s12",
                Label = "Entrada / Saída",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoEntradaSaida))),
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "change", Function = "fnChangeEntradaSaida" } }

            });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "tipoMovimentoId",
                Class = "col l6 m6 s12",
                Label = "Tipo de Movimento",
                Required = true,
                DataUrl = @Url.Action("TipoMovimento", "AutoComplete"),
                DataUrlPost = @Url.Action("NovoTipoMovimento"),
                LabelId = "tipoMovimentoDescricao",
                PreFilter = "tipoEntradaSaida"
            }, ResourceHashConst.EstoqueCadastrosTiposMovimento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "produtoId",
                Class = "col l12 m12 s12",
                Label = "Produto",
                Required = true,
                DataUrl = @Url.Action("Produto", "AutoComplete"),
                LabelId = "produtoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "Produto"),
                DataPostField = "descricao",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "blur", Function = "fnChangeProduto" }
                }
            }, ResourceHashConst.EstoqueCadastrosProdutos));

            config.Elements.Add(new InputTextUI { Id = "codigoProduto", Class = "col l4 m4 s12", Label = "Código", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "codigoBarras", Class = "col l4 m4 s12", Label = "Código de barras", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "saldoProduto", Class = "col l4 m4 s12", Label = "Saldo atual", Value = "0", Disabled = true });

            config.Elements.Add(new InputFloatUI
            {
                Id = "quantidade",
                Class = "col l6 m6 s12",
                Label = "Quantidade",
                Value = "0",
                Digits = 3,
                Required = true,
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeQuantidade" }
                }
            });

            config.Elements.Add(new InputNumbersUI { Id = "novoEstoque", Class = "col l6 m6 s12", Label = "Novo estoque", Value = "0", Disabled = true });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col l12 m12 s12", Label = "Observação", MaxLength = 200 });

            cfg.Content.Add(config);

            return cfg;
        }

        [HttpPost]
        [OperationRole(ResourceKey = ResourceHashConst.EstoqueCadastrosTiposMovimento, PermissionValue = EPermissionValue.Write)]
        public JsonResult NovoTipoMovimento(string term)
        {
            try
            {
                var tipoProduto = Request.QueryString["tipo"];

                var entity = new TipoMovimentoVM
                {
                    Descricao = term,
                    TipoEntradaSaida = tipoProduto
                };

                var resourceName = AppDefaults.GetResourceName(typeof(TipoMovimentoVM));
                var data = RestHelper.ExecutePostRequest<TipoMovimentoVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}