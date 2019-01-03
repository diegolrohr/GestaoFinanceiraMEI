using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Rest;
using Fly01.OrdemServico.ViewModel;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        public JsonResult ItemManutencao(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(descricao, '{term}') or contains(codigoProduto, '{term}') or contains(codigoBarras, '{term}')) and objetoDeManutencao eq {AppDefaults.APIEnumResourceName}ObjetoDeManutencao'Sim'");
            queryString.AddParam("$select", "id,descricao,codigoProduto,saldoProduto");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = $"Código produto: {item.CodigoProduto}" };

            return GetJson(filterObjects);
        }

        public JsonResult Vendedor(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(Core.ViewModels.Presentation.Commons.PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and vendedor eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<Core.ViewModels.Presentation.Commons.PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }

        public JsonResult Servico(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ServicoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"contains(descricao, '{term}') or contains(codigoServico, '{term}')");
            queryString.AddParam("$select", "id,descricao,codigoServico,valorServico");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ServicoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = $"Código Serviço: {item.CodigoServico}", valor = item.ValorServico };

            return GetJson(filterObjects);
        }

        public JsonResult ProdutoVenda(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ProdutoVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(descricao, '{term}') or contains(codigoProduto, '{term}') or contains(codigoBarras, '{term}')) and objetoDeManutencao eq {AppDefaults.APIEnumResourceName}ObjetoDeManutencao'Nao'");
            queryString.AddParam("$select", "id,descricao,codigoProduto,codigoBarras,valorVenda,saldoProduto");
            queryString.AddParam("$orderby", "descricao");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<ProdutoVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Descricao, detail = $"Código Produto: {item.CodigoProduto}", valor = item.ValorVenda };

            return GetJson(filterObjects);
        }
    }
}