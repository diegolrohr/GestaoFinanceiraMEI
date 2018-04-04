using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Financeiro.Domain;
using Fly01.Financeiro.Entities.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;

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

        //public JsonResult City(string term, string prefilter = "")
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(CityVM));

        //    Dictionary<string, string> queryString = new Dictionary<string, string>();
        //    queryString.Add("description", term);
        //    queryString.Add("justFields", "description,state");
        //    queryString.Add("order", "description");
        //    queryString.Add("active", "1");
        //    queryString.Add("max", "20");
        //    if (!string.IsNullOrWhiteSpace(prefilter))
        //    {
        //        queryString.AddParam("state", prefilter);
        //    }

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBaseFirst<CityVM>>(AppDefaults.UrlGateway.Replace("/v2/financeiro/", "/v1/"), resourceName, RestHelper.DefaultHeader, queryString).Data
        //                        select new { id = item.Description, label = item.Description, detail = item.State };

        //    return GetJson(filterObjects);
        //}

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
                ? $" and tipoCarteira eq Fly01.Financeiro.Domain.Enums.TipoCarteira'{prefilter}'"
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

            var filterTipoCarteira = "tipoCarteira eq Fly01.Financeiro.Domain.Enums.TipoCarteira'Despesa'";

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
                ? $" and tipoCarteira eq Fly01.Financeiro.Domain.Enums.TipoCarteira'{prefilter}'"
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
            queryString.AddParam("$select", "id,descricao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<FormaPagamentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao };

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

        //public JsonResult FinancialCategoryCorrelation(string term, string id_di, string portfolType)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);

        //    if (!String.IsNullOrWhiteSpace(id_di))
        //        queryString.Add("id_di", id_di);

        //    if (portfolType == "1" || portfolType == "7")
        //        queryString.Add("flow", "2");
        //    else
        //        queryString.Add("flow", "1");

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Descricao };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult FinancialCategoryCP(string term)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

        //    string fieldName = "descricao";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.Add("classe", "2");
        //    queryString.Add("flow", "1");

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Descricao };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult FinancialCategoryCR(string term)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

        //    string fieldName = "descricao";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.Add("classe", "2");
        //    queryString.Add("flow", "2");

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Descricao };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult PaymentformCP(string term)
        //{
        //    //var resourceName = AppDefaults.GetResourceName(typeof(CondicaoPagamentoVM));
        //    var resourceName = "PaymentForm";

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.Add("flow", "1");

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<FormaPagamentoVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, type = item.TipoFormaPagamento, label = item.Descricao };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult PaymentformCR(string term)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(FormaPagamentoVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.Add("flow", "2");

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<FormaPagamentoVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, type = item.TipoFormaPagamento, label = item.Descricao };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult PaymentConditionCP(string term, string paymentformId, string billValue)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.Add("flow", "1");

        //    queryString.Add("value", billValue.Replace(",", "."));
        //    queryString.Add("PaymentformId", paymentformId);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CondicaoParcelamentoVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Descricao };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult PaymentConditionCR(string term, string paymentformId, string billValue)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.Add("flow", "2");

        //    queryString.Add("value", billValue.Replace(",", "."));
        //    queryString.Add("PaymentformId", paymentformId);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CondicaoParcelamentoVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Descricao };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult BankCNAB(string term)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(ContaBancariaVM));

        //    string fieldName = "bankName";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.AddParam("CanGenerateCNAB", "1");

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, /*bankCode = item.Codigo, */label = item.NomeConta, detail = String.Format("Banco: {0}, Ag: {1}, Conta: {2}-{3}", /*item.Codigo, */ 1, item.Agencia, item.Conta, item.DigitoConta) };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult Bank(string term)
        //{
        //    //var resourceName = AppDefaults.GetResourceName(typeof(ContaBancariaVM));
        //    var resourceName = "Bank";

        //    string fieldName = "bankName";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, /*bankCode = item.Codigo,*/ label = item.NomeConta, detail = String.Format("Banco: {0}, Ag: {1}, Conta: {2}-{3}", /*item.Codigo*/ 1, item.Agencia, item.Conta, item.DigitoConta) };

        //    return GetJson(filterObjects);
        //}

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

        //public JsonResult BankFullData(string id)
        //{
        //    string resourceName = String.Format("{0}/{1}", AppDefaults.GetResourceName(typeof(BancoVM)), id);

        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(string.Empty, string.Empty);
        //    queryString.AddParam("expand", "");

        //    return GetJson(RestHelper.ExecuteGetRequest<BancoVM>(resourceName, queryString));
        //}

        //public JsonResult BankCode(string term)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(BankCodeVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<BankCodeVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Description, detail = String.Format("Código {0}", item.Id) };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult BankAgreement(string term, string bankCode)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(AgreementbankBusinessVM));

        //    string fieldName = "agreement";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.AddParam("bankcode", bankCode);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<AgreementbankBusinessVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Agreement };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult CardBrandByPaymentForm(string term, string paymentformId)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(CardbrandVM));

        //    string fieldName = "name";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.Add("paymentFormId", paymentformId);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CardbrandVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Name };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult PaymentProviderByPaymentForm(string term, string paymentformId)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(PaymentProviderVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.Add("paymentFormId", paymentformId);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PaymentProviderVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Description };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult CollectionSituation(string term)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(CollectionSituationVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CollectionSituationVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Description };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult InstructionType(string term, string bankCode)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(InstructionTypeVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.AddParam("bankCode", bankCode);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<InstructionTypeVM>>(resourceName, queryString).Data
        //                        select new { id = item.Item, label = item.Description };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult Wallet(string term, string bankCode)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(WalletVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.AddParam("bankCode", bankCode);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<WalletVM>>(resourceName, queryString).Data
        //                        select new { id = item.Item, label = item.Description };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult TypeInterest(string term, string bankCode)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(TypeInterestVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.AddParam("bankCode", bankCode);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<TypeInterestVM>>(resourceName, queryString).Data
        //                        select new { id = item.Item, label = item.Description };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult BankCodesInProtest(string term, string bankCode)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(BankCodesInProtestVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.AddParam("bankCode", bankCode);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<BankCodesInProtestVM>>(resourceName, queryString).Data
        //                        select new { id = item.Item, label = item.Description };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult DiscountType(string term, string bankCode)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(DiscountTypeVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);
        //    queryString.AddParam("bankCode", bankCode);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<DiscountTypeVM>>(resourceName, queryString).Data
        //                        select new { id = item.Item, label = item.Description };

        //    return GetJson(filterObjects);
        //}

        //public JsonResult Kind(string term)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(KindVM));

        //    string fieldName = "description";
        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(fieldName, term);

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<KindVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Description };

        //    return GetJson(filterObjects);
        //}
        #endregion
    }
}