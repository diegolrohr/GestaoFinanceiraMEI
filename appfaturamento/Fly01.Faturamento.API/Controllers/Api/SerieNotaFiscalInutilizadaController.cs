using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Faturamento.Domain.Entities;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("serienotafiscalinutilizada")]
    public class SerieNotaFiscalInutilizadaController : ApiPlataformaController<SerieNotaFiscal, SerieNotaFiscalInutilizadaBL>
    {
        public SerieNotaFiscalInutilizadaController()
        {

        }
    }
}