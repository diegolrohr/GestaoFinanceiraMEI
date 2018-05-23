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
        public JsonResult UnidadeMedida(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(UnidadeMedidaVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(abreviacao, '{term}')");
            queryString.AddParam("$select", "id,descricao,abreviacao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<UnidadeMedidaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = item.Abreviacao };

            return GetJson(filterObjects);
        }

        public JsonResult GrupoProduto(string term, string prefilter = "")
        {
            var resourceName = AppDefaults.GetResourceName(typeof(GrupoProdutoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}') and tipoProduto eq {AppDefaults.APIEnumResourceName}TipoProduto'{prefilter}'");
            queryString.AddParam("$select", "id,descricao,aliquotaIpi,ncmId,unidadeMedidaId");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<GrupoProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = "" };

            return GetJson(filterObjects);
        }

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

        public JsonResult Produto(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}') or " +
                                            $"contains(codigoProduto, '{term}') or " +
                                            $"contains(codigoBarras, '{term}')");

            queryString.AddParam("$select", "id,descricao,codigoProduto,codigoBarras,saldoProduto");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao };

            return GetJson(filterObjects);
        }

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

        public JsonResult EnquadramentoLegalIPI(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EnquadramentoLegalIpiVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigo, '{term}')");
            queryString.AddParam("$select", "id,codigo,descricao,grupoCST");
            queryString.AddParam("$orderby", "codigo");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EnquadramentoLegalIpiVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = string.Format("Grupo: {0} - Codigo: {1}", item.GrupoCST, item.Codigo)
                                };

            return GetJson(filterObjects);
        }
    }
}