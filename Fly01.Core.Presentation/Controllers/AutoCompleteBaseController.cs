using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using System.Collections.Generic;
using Fly01.Core.ViewModels;

namespace Fly01.Core.Presentation.Controllers
{
    [OperationRole(NotApply = true)]
    public class AutoCompleteBaseController : PrimitiveBaseController
    {
        protected JsonResult GetJson(object data) 
            => Json(data, JsonRequestBehavior.AllowGet);

        public JsonResult EstadoSemEX(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EstadoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(sigla, '{term}')) and sigla ne 'EX'");
            queryString.AddParam("$select", "id,nome,sigla,codigoIbge");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EstadoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.Sigla, estadoCodigoIbge = item.CodigoIbge };

            return GetJson(filterObjects);
        }

        public JsonResult Estado(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(EstadoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(nome, '{term}') or contains(sigla, '{term}')");
            queryString.AddParam("$select", "id,nome,sigla,codigoIbge");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<EstadoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.Sigla, estadoCodigoIbge = item.CodigoIbge };

            return GetJson(filterObjects);
        }

        public JsonResult Cidade(string term, string prefilter = "")
        {
            if (string.IsNullOrEmpty(prefilter)) return null;

            var resourceName = AppDefaults.GetResourceName(typeof(CidadeVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(nome, '{term}') and estadoId eq {prefilter}");
            queryString.AddParam("$select", "id,nome,estadoId,codigoIbge");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CidadeVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, estadoId = item.EstadoId, cidadeCodigoIbge = item.CodigoIbge };

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

        public JsonResult FormaPagamento(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(FormaPagamentoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", string.Format("contains(descricao, '{0}')", term));
            queryString.AddParam("$select", "id,descricao,tipoFormaPagamento");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<FormaPagamentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = EnumHelper.GetValue(typeof(TipoFormaPagamento), item.TipoFormaPagamento) };

            return GetJson(filterObjects);
        }

        public JsonResult CondicaoParcelamento(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}')");
            queryString.AddParam("$select", "id,descricao,condicoesParcelamento,qtdParcelas");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CondicaoParcelamentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, condicoesParcelamento = item.CondicoesParcelamento, qtdParcelas = item.QtdParcelas };

            return GetJson(filterObjects);
        }

        public JsonResult CategoriaPai(string term, string prefilter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

            var filterTipoCarteira = (prefilter != "")
                ? $"tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'{prefilter}'"
                : "";

            queryString.AddParam("$filter", $"contains(descricao, '{term}') and {filterTipoCarteira} and categoriaPaiId eq null");
            queryString.AddParam("$select", "id,descricao,categoriaPaiId,tipoCarteira");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, categoriaPaiId = item.CategoriaPaiId, level = item.Level };

            return GetJson(filterObjects);
        }

        public virtual JsonResult Categoria(string term, string filterTipoCarteira)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

            queryString.AddParam("$filter", $"contains(descricao, '{term}') {filterTipoCarteira}");
            queryString.AddParam("$select", "id,descricao,categoriaPaiId,tipoCarteira");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = item.TipoCarteira.ToString(), categoriaPaiId = item.CategoriaPaiId, level = item.Level };

            return GetJson(filterObjects);
        }

        //public JsonResult EstadoManagerNew(string term)
        //{
        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResponseDataNewGatewayVM<ManagerEstadoVM>>($"{AppDefaults.UrlManagerNew}", $"state/{term}")?.Data
        //                        select new { id = item.Id, label = item.Nome, detail = item.Sigla };

        //    return GetJson(filterObjects);
        //}

        ////http://10.51.8.188:3000/api/manager/city?term=porto&stateId=10
        //public JsonResult CidadeManagerNew(string term, string prefilter)
        //{
        //    var queryString = new Dictionary<string, string> {
        //        { "term", term },
        //        { "stateId", prefilter },
        //    };

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResponseDataNewGatewayVM<ManagerCidadeVM>>($"{AppDefaults.UrlManagerNew}", $"city", queryString)?.Data
        //                        select new { id = item.Id, label = item.Nome, detail = item.CodigoIbge };

        //    return GetJson(filterObjects);
        //}
    }
}