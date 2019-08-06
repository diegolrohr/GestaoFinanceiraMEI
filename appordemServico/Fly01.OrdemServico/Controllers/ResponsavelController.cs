using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.OrdemServicoCadastrosResponsaveis)]
    public class ResponsavelController : PessoaBaseController<PessoaVM>
    {
        protected override string ResourceTitle => "Responsável";
        protected override string LabelTitle => "Responsáveis Técnicos";
        protected override string Filter => "vendedor eq true";

        protected override void NormarlizarEntidade(ref PessoaVM entityVM)
        {
            entityVM.Vendedor = true;

            base.NormarlizarEntidade(ref entityVM);
        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Responsável",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmModalResponsavel",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "nome", Class = "col s12 l8", Label = "Razão Social / Nome Completo", Required = true, MaxLength = 100 });
            config.Elements.Add(new InputCpfcnpjUI { Id = "cpfcnpj", Class = "col s12 l4", Label = "CPF / CNPJ", Required = true, MaxLength = 18 });
            config.Elements.Add(new InputTextUI { Id = "nomeComercial", Class = "col s12 l12", Label = "Nome Comercial", Required = true, MaxLength = 100 });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "estadoId",
                Class = "col s6 l4",
                Label = "Estado",
                MaxLength = 35,
                DataUrl = Url.Action("Estado", "AutoComplete"),
                LabelId = "estadoNome",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeEstado" }
                }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "cidadeId",
                Class = "col s6 l4",
                Label = "Cidade (Escolha o estado antes)",
                MaxLength = 35,
                DataUrl = Url.Action("Cidade", "AutoComplete"),
                LabelId = "cidadeNome",
                PreFilter = "estadoId"
            });

            config.Elements.Add(new InputTextUI { Id = "endereco", Class = "col s6 l6", Label = "Endereço", Required = true, MaxLength = 50 });
            config.Elements.Add(new InputTextUI { Id = "numero", Class = "col s6 l6", Label = "Número", Required = true, MaxLength = 20 });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        protected override List<InputCheckboxUI> GetCheckBboxes()
        {
            return new List<InputCheckboxUI>
            {
               new InputCheckboxUI { Id = "cliente", Class = "col s12 l3", Label = "É Cliente" },
           };
        }
    }
}