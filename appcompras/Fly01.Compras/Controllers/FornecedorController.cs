using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using System.Collections.Generic;
using Fly01.Core.Presentation;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasCadastrosFornecedores)]
    public class FornecedorController : PessoaBaseController<PessoaVM>
    {
        protected override string ResourceTitle => "Fornecedor";
        protected override string LabelTitle => "Fornecedores";
        protected override string Filter => "fornecedor eq true";

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar fornecedor",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmModalFornecedor"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "nome", Class = "col s12 l8", Label = "Razão Social / Nome Completo", Required = true, MaxLength = 100 });
            config.Elements.Add(new InputCpfcnpjUI { Id = "cpfcnpj", Class = "col s12 l4", Label = "CPF / CNPJ", Required = true, MaxLength = 18 });
            config.Elements.Add(new InputTextUI { Id = "nomeComercial", Class = "col s12", Label = "Nome Comercial", Required = true, MaxLength = 100 });

            config.Elements.Add(new InputTextUI { Id = "endereco", Class = "col s6 l6", Label = "Endereço", Required = true, MaxLength = 50 });
            config.Elements.Add(new InputTextUI { Id = "numero", Class = "col s6 l6", Label = "Número", Required = true, MaxLength = 20 });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        protected override void NormarlizarEntidade(ref PessoaVM entityVM)
        {
            entityVM.Fornecedor = true;

            base.NormarlizarEntidade(ref entityVM);
        }

        protected override List<InputCheckboxUI> GetCheckBboxes()
        {
            return new List<InputCheckboxUI>
            {
               new InputCheckboxUI { Id = "cliente", Class = "col s12 l3", Label = "É Cliente" },
               new InputCheckboxUI { Id = "transportadora", Class = "col s12 l3", Label = "É Transportadora" },
               new InputCheckboxUI { Id = "vendedor", Class = "col s12 l3", Label = "É Vendedor/Resp. Técnico" },
               new InputCheckboxUI { Id = "consumidorFinal", Class = "col s12 l3", Label = "É Consumidor Final" }
           };
        }
    }
}