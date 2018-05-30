using System.Linq;
using System.Web.Mvc;
using Fly01.Core;
using System.Collections.Generic;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Compras.Controllers
{
    public class AutoCompleteController : AutoCompleteBaseController
    {
        public JsonResult Categoria(string term)
        {
            var filterTipoCarteira = $"and tipoCarteira eq {AppDefaults.APIEnumResourceName}TipoCarteira'Despesa'";

            return base.Categoria(term, filterTipoCarteira);
        }

        public JsonResult GrupoTributario(string term)
        {
            return base.GrupoTributario(term, $"and cfop/tipo eq {AppDefaults.APIEnumResourceName}TipoCfop'Entrada'");
        }

        public JsonResult Cfop(string term)
        {
            return base.Cfop(term, $"and tipo eq {AppDefaults.APIEnumResourceName}TipoCfop'Entrada'");
        }
    }
}