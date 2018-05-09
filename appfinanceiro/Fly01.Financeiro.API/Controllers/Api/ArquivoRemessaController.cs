using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.BL;
using System.Web.OData.Routing;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("arquivoremessa")]
    public class ArquivoRemessaController : ApiPlataformaController<ArquivoRemessa, ArquivoRemessaBL>
    {
    }
}