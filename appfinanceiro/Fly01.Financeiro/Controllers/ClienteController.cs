using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Financeiro.Controllers
{
    public class ClienteController : ClienteBaseController<PessoaVM>
    {
        public JsonResult PostCliente(string term)
        {
            var entity = new PessoaVM
            {
                Nome = term,
                Cliente = true,
                TipoIndicacaoInscricaoEstadual = "ContribuinteICMS"
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
    }
}