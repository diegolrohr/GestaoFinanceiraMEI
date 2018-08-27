using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.OrdemServico.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        //public override JsonResult Categoria(string term, string filterTipoCarteira)
        //{
        //    filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Despesa'";

        //    return base.Categoria(term, filterTipoCarteira);
        //}

        //public JsonResult CategoriaCP(string term)
        //{
        //    var filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Despesa'";

        //    return base.Categoria(term, filterTipoCarteira);
        //}

        //public JsonResult CategoriaCR(string term)
        //{
        //    var filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Receita'";

        //    return base.Categoria(term, filterTipoCarteira);
        //}

        //public JsonResult CondicaoParcelamentoAVista(string term)
        //{
        //    var resourceName = AppDefaults.GetResourceName(typeof(CondicaoParcelamentoVM));

        //    Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
        //    queryString.AddParam("$filter", string.Format("contains(descricao, '{0}') and ((qtdParcelas eq 1) or (condicoesParcelamento eq '0'))", term));
        //    queryString.AddParam("$select", "id,descricao");
        //    queryString.AddParam("$orderby", "descricao");

        //    var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<CondicaoParcelamentoVM>>(resourceName, queryString).Data
        //                        select new { id = item.Id, label = item.Descricao };

        //    return GetJson(filterObjects);
        //}

        public JsonResult Vendedor(string term)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"(contains(nome, '{term}') or contains(cpfcnpj, '{term}')) and vendedor eq true");
            queryString.AddParam("$select", "id,nome,cpfcnpj");
            queryString.AddParam("$orderby", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select new { id = item.Id, label = item.Nome, detail = item.CPFCNPJ == string.Empty ? "(Sem documento)" : item.CPFCNPJ };

            return GetJson(filterObjects);
        }
    }
}