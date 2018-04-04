using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.Entities.ViewModel;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;

namespace Fly01.Financeiro.Controllers
{
    public class ContaBancariaController : BaseController<ContaBancariaVM>
    {
        public ContaBancariaController()
        {
            ExpandProperties = "banco($select=id,nome)";
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
                conta = x.Conta +" - "+ x.DigitoConta,
                digitoConta = x.DigitoConta
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

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" });

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
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
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
                    List = @Url.Action("List")
                },
                UrlFunctions = Url.Action("Functions", "ContaBancaria", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m12 12",
                Label = "Banco",
                Required = true,
                DataUrl = @Url.Action("Banco", "AutoComplete"),
                LabelId = "bancoNome"
            });

            config.Elements.Add(new InputTextUI { Id = "nomeConta", Class = "col s4 m4 l6", Label = "Nome da Conta", Required = true, MaxLength = 150 });
            config.Elements.Add(new InputTextUI { Id = "agencia", Class = "col s3 m3 l2", Label = "Agência", Required = true, MinLength = 1, MaxLength = 10 });
            config.Elements.Add(new InputTextUI { Id = "digitoAgencia", Class = "col s1 m1 l1", Label = "Díg.", Required = true, MaxLength = 1 });
            config.Elements.Add(new InputTextUI { Id = "conta", Class = "col s3 m3 l2", Label = "Conta", Required = true, MinLength = 1, MaxLength = 10 });
            config.Elements.Add(new InputTextUI { Id = "digitoConta", Class = "col s1 m1 l1", Label = "Díg.", Required = true, MaxLength = 1 });


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
            };
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m12 12",
                Label = "Banco",
                Required = true,
                DataUrl = @Url.Action("Banco", "AutoComplete"),
                LabelId = "bancoNome"
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