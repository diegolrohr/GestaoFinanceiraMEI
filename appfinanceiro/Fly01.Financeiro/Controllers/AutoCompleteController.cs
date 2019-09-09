using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        public override JsonResult Categoria(string term, string filterTipoCarteira)
        {
            filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Despesa'";

            return base.Categoria(term, filterTipoCarteira);
        }

        public JsonResult CategoriaCP(string term)
        {
            var filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Despesa'";

            return base.Categoria(term, filterTipoCarteira);
        }

        public JsonResult CategoriaCR(string term)
        {
            var filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Receita'";

            return base.Categoria(term, filterTipoCarteira);
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

        public JsonResult ContaBancariaBanco(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ContaBancariaVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$expand", "banco($select=nome,codigo)");
            queryString.AddParam("$filter", string.Format("contains(nomeConta, '{0}') or contains(conta, '{0}')", term));
            queryString.AddParam("$select", "id, nomeConta, agencia, conta");
            queryString.AddParam("$orderby", "nomeConta");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.NomeConta,
                                    detail = $"Banco: {item.Banco.Nome} Agencia: {item.Agencia}-{item.DigitoAgencia} Conta: {item.Conta}-{item.DigitoConta}"
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

        public JsonResult Banco(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(BancoVM));
            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault("", "");
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