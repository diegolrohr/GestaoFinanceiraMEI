using Fly01.Estoque.ViewModel;
using Fly01.Core;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels;

namespace Fly01.Estoque.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        [OperationRole(ResourceKey = ResourceHash.EstoqueCadastrosProdutos, PermissionValue = EPermissionValue.Read)]
        public JsonResult ProdutoDescricao(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}')");
            queryString.AddParam("$select", "id,descricao,codigoProduto,saldoProduto");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = item.CodigoProduto, saldo = item.SaldoProduto };

            return GetJson(filterObjects);
        }

        [OperationRole(ResourceKey = ResourceHash.EstoqueCadastrosProdutos, PermissionValue = EPermissionValue.Read)]
        public JsonResult ProdutoCodigo(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(codigoProduto, '{term}')");
            queryString.AddParam("$select", "id,descricao,codigoProduto,saldoProduto");
            queryString.AddParam("$orderby", "codigoProduto");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.CodigoProduto, detail = item.Descricao, saldo = item.SaldoProduto };

            return GetJson(filterObjects);
        }

        [OperationRole(ResourceKey = ResourceHash.EstoqueCadastrosTiposMovimento, PermissionValue = EPermissionValue.Read)]
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