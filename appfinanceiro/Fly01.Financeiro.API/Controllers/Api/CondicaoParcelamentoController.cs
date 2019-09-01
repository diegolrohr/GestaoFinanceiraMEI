using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("condicaoparcelamento")]
    public class CondicaoParcelamentoController : ApiPlataformaController<CondicaoParcelamento, CondicaoParcelamentoBL>
    {
        public CondicaoParcelamentoController()
        {
        }
    }
}