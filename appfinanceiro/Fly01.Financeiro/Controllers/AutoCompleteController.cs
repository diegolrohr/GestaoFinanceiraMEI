using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Financeiro.Entities.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Rest;
using Fly01.Core.API;

namespace Fly01.Financeiro.Controllers
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

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault("", "");
            queryString.AddParam("$filter", string.Format("contains(nome, '{0}')", term));
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult Fornecedor(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault("", "");
            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and fornecedor eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult CategoriaPai(string term, string prefilter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

            var filterTipoCarteira = (prefilter != "")
                ? $" and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'{prefilter}'"
                : "";

            queryString.AddParam("$filter", $"contains(descricao, '{term}') {filterTipoCarteira} and categoriaPaiId eq null");
            queryString.AddParam("$select", "id,descricao,categoriaPaiId,tipoCarteira");
            //queryString.AddParam("$orderby", "tipoCarteira,categoriaPaiId,descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    categoriaPaiId = item.CategoriaPaiId,
                                    level = item.CategoriaPaiId == null ? 0 : 1
                                };

            return GetJson(filterObjects);
        }

        public JsonResult Categoria(string term)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

            var filterTipoCarteira = $"tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Despesa'";

            queryString.AddParam("$filter", $"contains(descricao, '{term}') and {filterTipoCarteira}");
            queryString.AddParam("$select", "id,descricao,categoriaPaiId,tipoCarteira");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, categoriaPaiId = item.CategoriaPaiId, level = item.Level };

            return GetJson(filterObjects);
        }

        private JsonResult Categoria(string term, string prefilter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

            var filterTipoCarteira = prefilter != ""
                ? $" and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'{prefilter}'"
                : "";

            queryString.AddParam("$filter", $"contains(descricao, '{term}')" + filterTipoCarteira);
            queryString.AddParam("$select", "id,descricao,categoriaPaiId");

            var filterObjects = from item
                                in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    categoriaPaiId = item.CategoriaPaiId,
                                    level = item.Level
                                };

            return GetJson(filterObjects);
        }

        public JsonResult CategoriaCP(string term)
        {
            return Categoria(term, "Despesa");
        }

        public JsonResult CategoriaCR(string term)
        {
            return Categoria(term, "Receita");
        }

        public JsonResult CategoriaVisualizar(string term)
        {
            return Categoria(term);
        }

        public JsonResult CondicaoParcelamento(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}')");
            queryString.AddParam("$select", "id,descricao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CondicaoParcelamentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao };

            return GetJson(filterObjects);
        }

        public JsonResult CondicaoParcelamentoAVista(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", string.Format("contains(descricao, '{0}') and ((qtdParcelas eq 1) or (condicoesParcelamento eq '0'))", term));
            queryString.AddParam("$select", "id,descricao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CondicaoParcelamentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao };

            return GetJson(filterObjects);
        }

        public JsonResult FormaPagamento(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(FormaPagamentoVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", string.Format("contains(descricao, '{0}')", term));
            queryString.AddParam("$select", "id,descricao,tipoFormaPagamento");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<FormaPagamentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = EnumHelper.SubtitleDataAnotation("TipoFormaPagamento", item.TipoFormaPagamento).Value };

            return GetJson(filterObjects);
        }

        public JsonResult ContaBancaria(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ContaBancariaVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", string.Format("contains(nomeConta, '{0}')", term));
            queryString.AddParam("$select", "id, nomeConta");//ver para add info banco
            queryString.AddParam("$orderby", "nomeConta");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.NomeConta };

            return GetJson(filterObjects);
        }

        public JsonResult ContaBancariaBanco(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ContaBancariaVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", string.Format("contains(nomeConta, '{0}') or contains(conta, '{0}')", term));
            queryString.AddParam("$select", "id, nomeConta, agencia, conta");
            queryString.AddParam("$expand", "banco($select=nome,codigo)");
            queryString.AddParam("$orderby", "nomeConta");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.NomeConta,
                                    detail = "Ag: " + item.Agencia +
                                             " Conta: " + item.Conta +
                                             " Banco: " + item.Banco.Codigo + " " + item.Banco.Nome
                                };

            return GetJson(filterObjects);
        }

        public JsonResult ContaBancariaConciliacao(string term)
        {
            //validar para não exibir já usadas em conciliacoes
            var resourceName = AppDefaults.GetResourceName(typeof(ContaBancariaVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", string.Format("contains(nomeConta, '{0}') or contains(conta, '{0}')", term));
            queryString.AddParam("$select", "id, nomeConta, agencia, conta");
            queryString.AddParam("$expand", "banco($select=nome,codigo)");
            queryString.AddParam("$orderby", "nomeConta");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.NomeConta,
                                    detail = "Ag: " + item.Agencia +
                                             " Conta: " + item.Conta +
                                             " Banco: " + item.Banco.Codigo + " " + item.Banco.Nome
                                };

            return GetJson(filterObjects);
        }

        public JsonResult Banco(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(BancoVM));
            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault("", "");
            queryString.AddParam("$filter", string.Format("contains(nome, '{0}') or contains(codigo, '{1}')", term, term));
            queryString.AddParam("$select", "id,codigo,nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<BancoVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    banco_codigo = item.Codigo,
                                    label = item.Nome,
                                    detail = String.Format("Banco: ({0}) - {1}", item.Codigo, item.Nome)
                                };

            return GetJson(filterObjects);
        }

        public JsonResult Cliente(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault("", "");
            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and cliente eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        #endregion
    }
}