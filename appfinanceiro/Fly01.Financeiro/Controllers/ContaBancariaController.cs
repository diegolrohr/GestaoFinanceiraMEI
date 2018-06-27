using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.Core.Presentation;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Enums;

namespace Fly01.Financeiro.Controllers
{
    public class ContaBancariaController : BaseController<ContaBancariaVM>
    {
        public ContaBancariaController()
        {
            ExpandProperties = "banco($select=id,nome,codigo)";
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$expand", ExpandProperties);
            customFilters.AddParam("$select", "id,bancoId,nomeConta,agencia,digitoAgencia,conta,digitoConta,registroFixo");

            return customFilters;
        }

        public override Func<ContaBancariaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                bancoId = x.BancoId,
                banco_nome = (x.Banco != null) ? x.Banco.Nome : "",
                nomeConta = x.NomeConta,
                agencia = x.Agencia,
                digitoAgencia = x.DigitoAgencia,
                conta = !string.IsNullOrEmpty(x.Conta) && !string.IsNullOrEmpty(x.DigitoConta) ? 
                    $"{x.Conta} - {x.DigitoConta}"
                    : string.Empty,
                digitoConta = x.DigitoConta,
                registroFixo = x.RegistroFixo,
                codigoCedente = x.CodigoCedente,
                codigoDV = x.CodigoDV, 
                taxaJuros = x.TaxaJuros, 
                percentualMulta = x.PercentualMulta
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Contas Bancárias",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "ContaBancaria", null, Request.Url.Scheme) + "?fns="
            };

            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions", "ContaBancaria", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" });

            config.Columns.Add(new DataTableUIColumn { DataField = "nomeConta", DisplayName = "Nome", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "banco_nome", DisplayName = "Banco", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "conta", DisplayName = "Conta - Digito", Priority = 3 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
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
                    Title = "Dados da conta bancária",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out },
                        new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Out },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main }
                    }
                },
                UrlFunctions = Url.Action("Functions", "ContaBancaria", null, Request.Url.Scheme) + "?fns="
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
                UrlFunctions = Url.Action("Functions", "ContaBancaria", null, Request.Url.Scheme) + "?fns=",
                ReadyFn = "fnFormReady",
                AfterLoadFn = "fnAfterLoad"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m12 12",
                Label = "Banco",
                Required = true,
                DataUrl = @Url.Action("Banco", "AutoComplete"),
                LabelId = "bancoNome",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnChangeBanco" },
                    new DomEventUI() { DomEvent = "autocompletechange", Function = "fnChangeBanco" }
                }
            });

            config.Elements.Add(new InputTextUI { Id = "nomeConta", Class = "col s4 m4 l6", Label = "Nome da Conta", Required = true, MaxLength = 150 });
            config.Elements.Add(new InputTextUI { Id = "agencia", Class = "col s3 m3 l2", Label = "Agência", Required = true, MinLength = 1, MaxLength = 10 });
            config.Elements.Add(new InputTextUI { Id = "digitoAgencia", Class = "col s1 m1 l1", Label = "Díg.", Required = true, MaxLength = 1 });
            config.Elements.Add(new InputTextUI { Id = "conta", Class = "col s3 m3 l2", Label = "Conta", Required = true, MinLength = 1, MaxLength = 10 });
            config.Elements.Add(new InputTextUI { Id = "digitoConta", Class = "col s1 m1 l1", Label = "Díg.", Required = true, MaxLength = 1 });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "checkCedente",
                Class = "col s12 l10",
                Label = "Informe o código do cedente",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnChangeCheckCedende" },
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "checkCedente",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Esta conta bancária emite boletos bancários? Marque para informar os dados referente a emissão de boletos.",
                    Position = TooltopUIPosition.Left
                }
            });

            config.Elements.Add(new InputTextUI { Id = "codigoCedente", Class = "col m12 l3", Label = "Código cedente", Required = false, MaxLength = 150 });
            config.Elements.Add(new InputTextUI { Id = "codigoDV", Class = "col m12 l3", Label = "CódigoDV", Required = false, MaxLength = 150 });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "taxaJuros",
                Class = "col m12 l3",
                Label = "Taxa de juros",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "taxaJuros",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe a taxa de juros que será cobrado por dia após o vencimento do boleto, do contrario, será utilizado a taxa de juros padão de 0.33% ao dia.",
                    Position = TooltopUIPosition.Top
                }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "percentualMulta",
                Class = "col m12 l3",
                Label = "Percentual Multa",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "percentualMulta",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe o percentual da multa que será cobrado por atraso após o vencimento do boleto, do contrario, será utilizado o percentual padão de 2.00%.",
                    Position = TooltopUIPosition.Top
                }
            });


            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar conta bancária",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),                    
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmContaBancaria",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnChangeBanco", "fnFormReady", "fnAfterLoad" }
            };
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m12 12",
                Label = "Banco",
                Required = true,
                DataUrl = @Url.Action("Banco", "AutoComplete"),
                LabelId = "bancoNome",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnChangeBanco" },
                    new DomEventUI() { DomEvent = "autocompletechange", Function = "fnChangeBanco" }
                }
            });

            config.Elements.Add(new InputTextUI { Id = "nomeConta", Class = "col s4 m4 l6", Label = "Nome da Conta", Required = true, MaxLength = 150 });
            config.Elements.Add(new InputTextUI { Id = "agencia", Class = "col s3 m3 l2", Label = "Agência", Required = true, MinLength = 1, MaxLength = 10 });
            config.Elements.Add(new InputTextUI { Id = "digitoAgencia", Class = "col s1 m1 l1", Label = "Díg.", Required = true, MaxLength = 1 });
            config.Elements.Add(new InputTextUI { Id = "conta", Class = "col s3 m3 l2", Label = "Conta", Required = true, MinLength = 1, MaxLength = 10 });
            config.Elements.Add(new InputTextUI { Id = "digitoConta", Class = "col s1 m1 l1", Label = "Díg.", Required = true, MaxLength = 1 });
            
            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

    }
}