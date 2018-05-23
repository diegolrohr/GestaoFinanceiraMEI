using System.Collections.Generic;
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

        public JsonResult CondicaoParcelamento(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", string.Format("contains(descricao, '{0}')", term));
            queryString.AddParam("$select", "id,descricao");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CondicaoParcelamentoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao };

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

        public JsonResult Transportadora(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault("", "");
            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and transportadora eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult Categoria(string term)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));

            var filterTipoCarteira = $"tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Receita'";

            queryString.AddParam("$filter", $"contains(descricao, '{term}') and {filterTipoCarteira}");
            queryString.AddParam("$select", "id,descricao,categoriaPaiId,tipoCarteira");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CategoriaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, categoriaPaiId = item.CategoriaPaiId, level = item.Level };

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

        public JsonResult Produto(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoVM));

            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigoProduto, '{term}') or contains(codigoBarras, '{term}')");
            queryString.AddParam("$select", "id,descricao,valorCusto,codigoProduto,codigoBarras");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, valor = item.ValorCusto };

            return GetJson(filterObjects);
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
                                    //detail = string.Format("Grupo: {0} - Descricao: {1}", item.GrupoCST, item.Descricao)
                                    detail = string.Format("Grupo: {0} - Código: {1}", item.GrupoCST, item.Codigo)
                                };

            return GetJson(filterObjects);
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