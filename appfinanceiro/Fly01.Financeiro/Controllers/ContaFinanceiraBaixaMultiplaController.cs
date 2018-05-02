using Fly01.Financeiro.Controllers.Base;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes.Helpers;

namespace Fly01.Financeiro.Controllers
{
    public class ContaFinanceiraBaixaMultiplaController : BaseController<ContaFinanceiraBaixaMultiplaVM>
    {
        public ContaFinanceiraBaixaMultiplaController()
        {
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override Func<ContaFinanceiraBaixaMultiplaVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        //TODO: ver questao das rotas e renomear as controllers e classes

        public ContentResult FormBaixaMultipla(string tipoConta)
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
                    Title = "Baixas múltiplas de contas a " + (tipoConta == "CP" ? "pagar" : "receber"),
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" + tipoConta },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = Url.Action("Create"),
                    Get = Url.Action("Json") + "/",
                    List = @Url.Action("List", tipoConta == "CP" ? "ContaPagar" : "ContaReceber")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnFormReadyBaixaMultipla"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "contasFinanceirasGuids" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoContaFinanceira", Value = tipoConta == "CP" ? "ContaPagar" : "ContaReceber" });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "contaBancariaId",
                Class = "col s12 l6",
                Label = "Conta Bancária",
                Required = true,
                DataUrl = @Url.Action("ContaBancaria", "AutoComplete"),
                LabelId = "contaBancariaDescricao",
                DataUrlPostModal = Url.Action("FormModal", "ContaBancaria"),
                DataPostField = "nomeConta"
            });
            
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m6", Label = "Data da Baixa", Required = true, Value = DateTime.Now.ToString("dd/MM/yyyy") });
            config.Elements.Add(new TextareaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });
            config.Elements.Add(new InputCurrencyUI { Id = "somaValoresSelecionados", Class = "col s12 m6", Label = "Total das Baixas", Value = "0", Disabled = true });
            config.Elements.Add(new InputNumbersUI { Id = "countContasSelecionadas", Class = "col s12 m6", Label = "Contas Selecionadas", Value = "0", Disabled = true });

            config.Elements.Add(new LabelsetUI { Id = "contasFinanceirasLabel", Class = "col s12", Label = "Selecione as contas que deseja baixar" });
            
            cfg.Content.Add(config);


            DataTableUI dtcfg = new DataTableUI
            {
                Id = "dtContasExistentes",
                UrlGridLoad = Url.Action("GridLoadContasNaoPagas", (tipoConta == "CP" ? "ContaPagar" : "ContaReceber")),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig()
                {
                    Select = new { style = "multi" },
                    WithoutRowMenu = true,
                    PageLength = 50
                },
                //Callbacks = new DataTableUICallbacks()
                //{
                //    RowCallback = "fnRowCallbackContasExistentes"
                //}
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
            #endregion

            cfg.Content.Add(dtcfg);
            
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

    }
}