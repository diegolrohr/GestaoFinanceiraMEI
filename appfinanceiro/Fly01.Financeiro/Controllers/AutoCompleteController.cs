using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Financeiro.ViewModel;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Financeiro.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        public JsonResult CategoriaPai(string term, string prefilter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

            var filterTipoCarteira = (prefilter != "")
                ? $" and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'{prefilter}'"
                : "";

            queryString.AddParam("$filter", $"contains(descricao, '{term}') {filterTipoCarteira} and categoriaPaiId eq null");
            queryString.AddParam("$select", "id,descricao,categoriaPaiId,tipoCarteira");
            
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
                                select new { id = item.Id, label = item.Descricao, detail = EnumHelper.GetValue(typeof(TipoFormaPagamento), item.TipoFormaPagamento) };

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

        public JsonResult ContaBancariaBancoEmiteBoleto(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ContaBancariaVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$expand", "banco($select=nome)");
            queryString.AddParam("$filter", $"contains(banco/nome, '{term}') and banco/emiteBoleto eq true");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Banco.Nome
                                };

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
                                    detail = $"Ag: {item.Agencia} Conta: {item.Conta} Banco : {item.Banco.Codigo} {item.Banco.Nome}"
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
                                    detail = $"Ag: {item.Agencia} Conta: {item.Conta} Banco : {item.Banco.Codigo} {item.Banco.Nome}"
                                };

            return GetJson(filterObjects);
        }

        public JsonResult Banco(string term, bool emiteBoleto = false)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(BancoVM));
            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault("", "");
            if (emiteBoleto)
                queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(codigo, '{term}')) and emiteBoleto eq true");
            else
                queryString.AddParam("$filter", $"contains(nome, '{term}') or contains(codigo, '{term}')");

            queryString.AddParam("$select", "id,codigo,nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<BancoVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    banco_codigo = item.Codigo,
                                    label = item.Nome,
                                    detail = $"Banco: {item.Codigo} - {item.Nome}",
                                    agenciaContaRequired = item.Codigo != "999"
                                };

            return GetJson(filterObjects);
        }
    }
}