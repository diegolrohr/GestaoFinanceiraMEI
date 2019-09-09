using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using System.Collections.Generic;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroCadastrosClientes)]
    public class ClienteController : PessoaBaseController<PessoaVM>
    {
        protected override string ResourceTitle => "Cliente";
        protected override string LabelTitle => "Clientes";
        protected override string Filter => "cliente eq true";

        protected override void NormarlizarEntidade(ref PessoaVM entityVM)
        {
            entityVM.Cliente = true;

            base.NormarlizarEntidade(ref entityVM);
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public JsonResult PostCliente(string term)
        {
            var entity = new PessoaVM
            {
                Nome = term,
                Cliente = true
            };

            NormarlizarEntidade(ref entity);

            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
                var data = RestHelper.ExecutePostRequest<PessoaVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected override List<InputCheckboxUI> GetCheckBboxes()
        {
            return new List<InputCheckboxUI>
            {
               new InputCheckboxUI { Id = "fornecedor", Class = "col s12 l3", Label = "É Fornecedor" },
               new InputCheckboxUI { Id = "transportadora", Class = "col s12 l3", Label = "É Transportadora" },
               new InputCheckboxUI { Id = "vendedor", Class = "col s12 l3", Label = "É Vendedor/Resp. Técnico" },
               new InputCheckboxUI { Id = "consumidorFinal", Class = "col s12 l3", Label = "É Consumidor Final" }
           };
        }
    }
}