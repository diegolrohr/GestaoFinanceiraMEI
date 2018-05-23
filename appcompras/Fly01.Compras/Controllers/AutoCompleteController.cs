using System.Linq;
using System.Web.Mvc;
using Fly01.Core;
using System.Collections.Generic;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Compras.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        public JsonResult Categoria(string term)
        {
            var filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Despesa'";

            return base.Categoria(term, filterTipoCarteira);
        }

        public JsonResult GrupoTributario(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(GrupoTributarioVM));
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') and cfop/tipo eq {AppDefaults.APIEnumResourceName}TipoCfop'Entrada'");
            queryString.AddParam("$select", "id,descricao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<GrupoTributarioVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = "" };

            return GetJson(filterObjects);
        }

        public JsonResult Cfop(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(CfopVM));
            int codigo = 0;
            int.TryParse(term, out codigo);

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"(contains(descricao, '{term}') or codigo eq {codigo}) and tipo eq {AppDefaults.APIEnumResourceName}TipoCfop'Entrada'");
            queryString.AddParam("$select", "id,descricao,codigo");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CfopVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = item.Codigo };

            return GetJson(filterObjects);
        }
    }
}