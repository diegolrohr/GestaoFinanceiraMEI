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
using Fly01.Core.Rest;
using Fly01.Core;
using System.Linq;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
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
            customFilters.AddParam("$select", "id,bancoId,nomeConta,agencia,digitoAgencia,conta,digitoConta");

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
                digitoConta = x.DigitoConta
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Contas Bancárias",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "ContaBancaria", null, Request.Url.Scheme) + "?fns="
            };

            var config = new DataTableUI { Id = "fly01dt", UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions", "ContaBancaria", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "nomeConta", DisplayName = "Nome", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "banco_nome", DisplayName = "Banco", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "conta", DisplayName = "Conta - Digito", Priority = 3 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
            target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });


            return target;
        }

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
                    Title = "Conta Bancária",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "ContaBancaria", null, Request.Url.Scheme) + "?fns="
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
                UrlFunctions = Url.Action("Functions", "ContaBancaria", null, Request.Url.Scheme) + "?fns=",
                ReadyFn = "fnFormReady",
                AfterLoadFn = "fnAfterLoad"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "bancoId",
                Class = "col s12",
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

            config.Elements.Add(new InputTextUI { Id = "nomeConta", Class = "col s12 m6 l6", Label = "Nome da Conta", Required = true, MaxLength = 150 });
            config.Elements.Add(new InputTextUI { Id = "agencia", Class = "col s4 m2 l2", Label = "Agência", Required = true, MinLength = 1, MaxLength = 10 });
            config.Elements.Add(new InputTextUI { Id = "digitoAgencia", Class = "col s2 m1 l1", Label = "Díg.", Required = false, MaxLength = 1 });
            config.Elements.Add(new InputTextUI { Id = "conta", Class = "col s4 m2 l2", Label = "Conta", Required = true, MinLength = 1, MaxLength = 10 });
            config.Elements.Add(new InputTextUI { Id = "digitoConta", Class = "col s2 m1 l1", Label = "Díg.", Required = true, MaxLength = 1 });

            //config.Elements.Add(new InputCheckboxUI
            //{
            //    Id = "inicializaContaBancaria",
            //    Class = "col s4 l3",
            //    Label = " Inicializar conta com Saldo",
            //    DomEvents = new List<DomEventUI>()
            //    {
            //        new DomEventUI() { DomEvent = "change", Function = "fnChangeCheckInicializa" },
            //    }
            //});

            //config.Elements.Add(new InputCurrencyUI { Id = "valorInicial", Class = "col s4 l3", Label = "Valor", Disabled = true });

            cfg.Content.Add(config);

            return cfg;
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
                Functions = new List<string>() { "fnChangeBanco", "fnFormReady", "fnAfterLoad", "fnChangeCheckInicializa" }
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
                Id = "inicializaContaBancaria",
                Class = "col s4 l3",
                Label = " Inicializar conta com Saldo",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnChangeCheckInicializa" },
                }
            });

            config.Elements.Add(new InputCurrencyUI { Id = "valorInicial", Class = "col s4 l3", Label = "Valor", Disabled = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}