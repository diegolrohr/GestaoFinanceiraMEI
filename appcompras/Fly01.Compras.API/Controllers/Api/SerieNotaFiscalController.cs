using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Compras.BL;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("serienotafiscal")]
    public class SerieNotaFiscalController : ApiPlataformaController<SerieNotaFiscal, SerieNotaFiscalBL>
    {
        public SerieNotaFiscalController()
        {

        }
    }
}