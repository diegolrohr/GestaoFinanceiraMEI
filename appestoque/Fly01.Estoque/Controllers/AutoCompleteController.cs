using Fly01.Estoque.Controllers.Base;
using Fly01.Estoque.Entities.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    public class AutoCompleteController : Controller
    {
        #region Private Methods
        private JsonResult GetJson(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Public Methods

        public JsonResult Estado(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EstadoVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(nome, '{term}') or contains(sigla, '{term}')");
            queryString.AddParam("$select", "id,nome,sigla");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EstadoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.Sigla };

            return GetJson(filterObjects);
        }

        public JsonResult Cidade(string term, string prefilter = "")
        {
            if (string.IsNullOrEmpty(prefilter)) return null;

            var resourceName = AppDefaults.GetResourceName(typeof(CidadeVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(nome, '{term}') and estadoId eq {prefilter}");
            queryString.AddParam("$select", "id,nome,estadoId");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CidadeVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, estadoId = item.EstadoId };

            return GetJson(filterObjects);
        }

        public JsonResult City(string term, string prefilter = "")
        {
            var resourceName = AppDefaults.GetResourceName(typeof(CityVM));

            Dictionary<string, string> queryString = new Dictionary<string, string>();
            queryString.Add("description", term);
            queryString.Add("justFields", "description,state");
            queryString.Add("order", "description");
            queryString.Add("active", "1");
            queryString.Add("max", "20");
            if (!string.IsNullOrWhiteSpace(prefilter))
            {
                queryString.AddParam("state", prefilter);
            }

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBaseFirst<CityVM>>(AppDefaults.UrlGateway.Replace("/v2/estoque/", "/v1/"), resourceName, RestHelper.DefaultHeader, queryString).Data
                                select new { id = item.Description, label = item.Description, detail = item.State };

            return GetJson(filterObjects);
        }

        public JsonResult Ncm(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(NCMVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigo, '{term}')");
            queryString.AddParam("$select", "id,codigo,descricao,aliquotaIPI");
            queryString.AddParam("$orderby", "codigo");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<NCMVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = string.Format("Cod: {0} - Alíquota IPI: {1}", item.Codigo, (item.AliquotaIPI / 100).ToString("P", AppDefaults.CultureInfoDefault))
                                };

            return GetJson(filterObjects);
        }

        public JsonResult Cest(string term, string prefilter = "")
        {
            if (string.IsNullOrEmpty(prefilter)) return null;

            var resourceName = AppDefaults.GetResourceName(typeof(CestVM));

            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(descricao, '{term}') or contains(codigo, '{term}')) and ncmId eq {prefilter}");
            queryString.AddParam("$select", "id,codigo,descricao,segmento");
            queryString.AddParam("$orderby", "codigo");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CestVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = string.Format("Cód: {0} - Segmento: {1}", item.Codigo, item.Segmento)
                                };

            return GetJson(filterObjects);
        }

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

            queryString.AddParam("$filter", $"contains(descricao, '{term}') and tipoProduto eq Fly01.Estoque.Domain.Enums.TipoProduto'{prefilter}'");
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
                                            $"tipoEntradaSaida eq {BaseController<TipoMovimentoVM>.EstoqueAPIEnumResourceName} TipoEntradaSaida'{prefilter}'");

            queryString.AddParam("$select", "id,descricao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<TipoMovimentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao };

            return GetJson(filterObjects);
        }

        public JsonResult EnquadramentoLegalIPI(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EnquadramentoLegalIPIVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigo, '{term}')");
            queryString.AddParam("$select", "id,codigo,descricao,grupoCST");
            queryString.AddParam("$orderby", "codigo");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EnquadramentoLegalIPIVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = string.Format("Grupo: {0} - Codigo: {1}", item.GrupoCST, item.Codigo)
                                };

            return GetJson(filterObjects);
        }

        #endregion
    }
}