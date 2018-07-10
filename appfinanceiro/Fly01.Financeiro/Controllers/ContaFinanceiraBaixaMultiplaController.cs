using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.uiJS.Enums;

namespace Fly01.Financeiro.Controllers
{
    public class ContaFinanceiraBaixaMultiplaController : BaseController<ContaFinanceiraBaixaMultiplaVM>
    {
        public string ContaBancariaResourceHash { get; set; }

        public ContaFinanceiraBaixaMultiplaController(string contaBancariaResourceHash)
        {
            ContaBancariaResourceHash = contaBancariaResourceHash;
        }

        public override ContentResult List() { throw new NotImplementedException(); }

        public override Func<ContaFinanceiraBaixaMultiplaVM, object> GetDisplayData() { throw new NotImplementedException(); }

        public override ContentResult Form() { throw new NotImplementedException(); }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });
            }

            return target;
        }

        protected ContentResult FormBaixaMultipla(string tipoConta)
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
                    Title = "Baixas múltiplas de contas a " + tipoConta.ToLower(),
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())                   
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = Url.Action("Create"),
                    Get = Url.Action("Json") + "/",
                    List = @Url.Action("List", "Conta" + tipoConta)
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReadyBaixaMultipla"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "contasFinanceirasGuids" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoContaFinanceira", Value = "Conta" + tipoConta });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "contaBancariaId",
                Class = "col s12 m6",
                Label = "Conta Bancária",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBanco", "AutoComplete"),
                LabelId = "contaBancariaDescricao",
                DataUrlPostModal = Url.Action("FormModal", "ContaBancaria"),
                DataPostField = "nomeConta"
            }, ContaBancariaResourceHash));

            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m6", Label = "Data da Baixa", Required = true, Value = DateTime.Now.ToString("dd/MM/yyyy") });
            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });
            config.Elements.Add(new InputCurrencyUI { Id = "somaValoresSelecionados", Class = "col s12 m6", Label = "Total das Baixas", Value = "0", Disabled = true });
            config.Elements.Add(new InputNumbersUI { Id = "countContasSelecionadas", Class = "col s12 m6", Label = "Contas Selecionadas", Value = "0", Disabled = true });

            config.Elements.Add(new LabelSetUI { Id = "contasFinanceirasLabel", Class = "col s12", Label = "Selecione as contas que deseja baixar" });
            config.Elements.Add(new ButtonGroupUI
            {
                Id = "selectAllBtnGrp",
                Class = "col s12 m6 offset-m3",
                OnClickFn = "fnSelectsAllClick",
                Options = new List<ButtonGroupOptionUI>
                {
                    new ButtonGroupOptionUI {Id = "btnSelectAll", Value = "selectAll", Label = "Selecionar página"},
                    new ButtonGroupOptionUI {Id = "btnDeselectAll", Value = "deselectAll", Label = "Deselecionar página"},
                }
            });

            cfg.Content.Add(config);

            DataTableUI dtcfg = new DataTableUI
            {
                Id = "dtContasExistentes",
                UrlGridLoad = Url.Action("GridLoadContasNaoPagas", "Conta" + tipoConta),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig()
                {
                    Select = new { style = "multi" },
                    WithoutRowMenu = true,
                    PageLength = 50
                }
            };

            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Nº", Priority = 1 });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 4 });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Pessoa", Priority = 3 });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Vencimento", Priority = 5, Type = "date" });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "descricaoParcela", DisplayName = "Parcela", Priority = 6 });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "valorPrevisto", DisplayName = "Valor", Priority = 7, Type = "currency" });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "saldo", DisplayName = "Saldo", Priority = 2, Orderable = false, Searchable = false });

            #region Helpers
            config.Helpers.Add(new TooltipUI
            {
                Id = "countContasSelecionadas",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Número de contas selecionadas. O número máximo permitido é de 50."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "contasFinanceirasLabel",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Basta clicar nas contas desejadas abaixo. É listado as contas com status Em Aberto ou status Baixado Parcialmente."
                }
            });
            #endregion

            cfg.Content.Add(dtcfg);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }
    }
}