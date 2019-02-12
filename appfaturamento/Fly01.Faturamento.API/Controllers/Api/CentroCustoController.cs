using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("centrocusto")]
    public class CentroCustoController : ApiPlataformaController<CentroCusto, CentroCustoBL>
    {
        public CentroCustoController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}