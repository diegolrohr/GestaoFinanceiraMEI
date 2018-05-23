﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Faturamento.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Faturamento.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        public JsonResult Nbs(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(NBSVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigo, '{term}')");
            queryString.AddParam("$select", "id,codigo,descricao");
            queryString.AddParam("$orderby", "codigo");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<NBSVM>>(resourceName, queryString).Data
                                select new
                                {
                                    id = item.Id,
                                    label = item.Descricao,
                                    detail = string.Format("Código {0}", item.Codigo)
                                };

            return GetJson(filterObjects);
        }

        public JsonResult GrupoTributario(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(GrupoTributarioVM));
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') and cfop/tipo eq {AppDefaults.APIEnumResourceName}TipoCfop'Saida'");
            queryString.AddParam("$select", "id,descricao,tipoTributacaoICMS");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<GrupoTributarioVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = "", tipoTributacaoICMS = item.TipoTributacaoICMS };

            return GetJson(filterObjects);
        }

        public JsonResult Categoria(string term)
        {
            var filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Receita'";

            return base.Categoria(term, filterTipoCarteira);
        }

        public JsonResult Servico(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ServicoVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigoServico, '{term}')");
            queryString.AddParam("$select", "id,descricao,valorServico");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ServicoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, valor = item.ValorServico };

            return GetJson(filterObjects);
        }

        public JsonResult Cfop(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(CfopVM));
            int codigo = 0;
            int.TryParse(term, out codigo);

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"(contains(descricao, '{term}') or codigo eq {codigo}) and tipo eq {AppDefaults.APIEnumResourceName}TipoCfop'Saida'");
            queryString.AddParam("$select", "id,descricao,codigo");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CfopVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = item.Codigo };

            return GetJson(filterObjects);
        }

        public JsonResult SerieNotaFiscal(string term, string tipo)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(SerieNotaFiscalVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam(
                "$filter", $"contains(serie, '{term}') and (tipoOperacaoSerieNotaFiscal eq {AppDefaults.APIEnumResourceName}TipoOperacaoSerieNotaFiscal'{tipo}'" +
                $" or tipoOperacaoSerieNotaFiscal eq {AppDefaults.APIEnumResourceName}TipoOperacaoSerieNotaFiscal'Ambas')" +
                $" and statusSerieNotaFiscal eq {AppDefaults.APIEnumResourceName}StatusSerieNotaFiscal'Habilitada'"
                );
            queryString.AddParam("$orderby", "serie");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<SerieNotaFiscalVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Serie.ToUpper(), detail = "Próximo número: " + item.NumNotaFiscal.ToString(), numNotaFiscal = item.NumNotaFiscal };

            return GetJson(filterObjects);
        }

        public JsonResult SerieNFe(string term)
        {
            return SerieNotaFiscal(term, "NFe");
        }

        public JsonResult SerieNFSe(string term)
        {
            return SerieNotaFiscal(term, "NFSe");
        }

        public JsonResult ProdutoOrdem(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigoProduto, '{term}') or contains(codigoBarras, '{term}')");
            queryString.AddParam("$select", "id,descricao,valorVenda,codigoProduto,codigoBarras");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, valor = item.ValorVenda };

            return GetJson(filterObjects);
        }
    }
}