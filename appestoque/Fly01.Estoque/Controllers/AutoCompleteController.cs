using Fly01.Estoque.ViewModel;
using Fly01.Core;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Estoque.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        public JsonResult TipoMovimento(string term, string prefilter = "")
        {
            var resourceName = AppDefaults.GetResourceName(typeof(TipoMovimentoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}') and " +
                                            $"tipoEntradaSaida eq {AppDefaults.APIEnumResourceName}TipoEntradaSaida'{prefilter}'");
            
            queryString.AddParam("$select", "id,descricao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<TipoMovimentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao };

            return GetJson(filterObjects);
        }
    }
}