using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Collections.Generic;

namespace Fly01.Core.Presentation.Controllers
{
    public class AutoCompleteBaseController : Controller
    {
        protected JsonResult GetJson(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

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

        public JsonResult EstadoSigla(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EstadoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(nome, '{term}')");
            queryString.AddParam("$select", "nome,sigla");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EstadoVM>>(resourceName, queryString).Data
                                select new { id = item.Sigla.ToUpper(), label = item.Nome, detail = item.Sigla };

            return GetJson(filterObjects);
        }

        public JsonResult Ncm(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(NcmVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigo, '{term}')");
            queryString.AddParam("$select", "id,codigo,descricao,aliquotaIPI");
            queryString.AddParam("$orderby", "codigo");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<NcmVM>>(resourceName, queryString).Data
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
                                    detail = $"Código: {item.Codigo} - Segmento: {item.Segmento}"
                                };

            return GetJson(filterObjects);
        }

        public JsonResult Pessoa(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(nome, '{term}') or contains(cpfcnpj, '{term}')");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult Fornecedor(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and fornecedor eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult Cliente(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and cliente eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }


    }
}