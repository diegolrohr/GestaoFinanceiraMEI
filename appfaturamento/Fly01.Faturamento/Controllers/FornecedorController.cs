using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.uiJS.Classes.Elements;
using System.Collections.Generic;
using Fly01.Core.Presentation;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoCadastrosFornecedores)]
    public class FornecedorController : PessoaBaseController<PessoaVM>
    {
        protected override string ResourceTitle => "Fornecedor";
        protected override string LabelTitle => "Fornecedores";
        protected override string Filter => "fornecedor eq true";

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