using Fly01.Core;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroCadastrosCondicoesParcelamento)]
    public class CondicaoParcelamentoController : CondicaoParcelamentoBaseController<CondicaoParcelamentoVM>
    {
        [OperationRole(NotApply = true)]
        public JsonResult PostCondicaoParcelamento(string term)
        {
            var entity = new CondicaoParcelamentoVM
            {
                Descricao = term,
                QtdParcelas = 1,
            };
            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));
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