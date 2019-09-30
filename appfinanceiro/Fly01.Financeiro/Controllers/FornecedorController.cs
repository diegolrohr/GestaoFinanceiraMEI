using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.uiJS.Classes.Elements;
using System.Collections.Generic;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
    public class FornecedorController : PessoaBaseController<PessoaVM>
    {
        protected override string ResourceTitle => "Fornecedor";
        protected override string LabelTitle => "Fornecedores";
        protected override string Filter => "fornecedor eq true";

        public JsonResult PostFornecedor(string term)
        {
            var entity = new PessoaVM
            {
                Nome = term,
                Fornecedor = true
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