using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Presentation.Controllers;
using System.Collections.Generic;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Rest;
using System.Linq;
using Fly01.Compras.Helpers;

namespace Fly01.Compras.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        public override JsonResult Categoria(string term, string filterTipoCarteira)
        {
            filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Despesa'";

            return base.Categoria(term, filterTipoCarteira);
        }

        public JsonResult ProdutoOrdem(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"(contains(descricao, '{term}') or contains(codigoProduto, '{term}') or contains(codigoBarras, '{term}')) and (objetoDeManutencao eq {AppDefaults.APIEnumResourceName}ObjetoDeManutencao'Nao')");
            queryString.AddParam("$select", "id,descricao,valorCusto,codigoProduto,codigoBarras");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, valor = item.ValorCusto };

            return GetJson(filterObjects);
        }

        public JsonResult SerieNotaFiscal(string term, string tipo)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(SerieNotaFiscalVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam(
                "$filter", $"contains(serie, '{term}') and (tipoOperacaoSerieNotaFiscal eq {AppDefaults.APIEnumResourceName}TipoOperacaoSerieNotaFiscal'{tipo}'" +
                $" or tipoOperacaoSerieNotaFiscal eq {AppDefaults.APIEnumResourceName}TipoOperacaoSerieNotaFiscal'Ambas')"
                );
            queryString.AddParam("$orderby", "serie");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<SerieNotaFiscalVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Serie.ToUpper(), detail = "Próximo número: " + item.NumNotaFiscal.ToString(), numNotaFiscal = item.NumNotaFiscal };

            return GetJson(filterObjects);
        }

        public JsonResult SerieNFe(string term)
            => SerieNotaFiscal(term, "NFe");


        public JsonResult FornecedorXML(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and fornecedor eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.CPFCNPJ != string.Empty ? item.Nome + " / " + FormatTextHelper.FormatCpfCnpj(item.CPFCNPJ) : item.Nome,
                                    detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : FormatTextHelper.FormatCpfCnpj(item.CPFCNPJ)
                                };

            return GetJson(filterObjects);
        }

        public JsonResult TransportadoraXML(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and transportadora eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.CPFCNPJ != string.Empty ? item.Nome + " / " + FormatTextHelper.FormatCpfCnpj(item.CPFCNPJ) : item.Nome,
                                    detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : FormatTextHelper.FormatCpfCnpj(item.CPFCNPJ)
                                };

            return GetJson(filterObjects);
        }
    }
}