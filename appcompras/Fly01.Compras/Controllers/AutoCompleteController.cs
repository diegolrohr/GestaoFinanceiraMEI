using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Compras.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        public override JsonResult Categoria(string term, string filterTipoCarteira)
        {
            filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Despesa'";

            return base.Categoria(term, filterTipoCarteira);
        }
    }
}