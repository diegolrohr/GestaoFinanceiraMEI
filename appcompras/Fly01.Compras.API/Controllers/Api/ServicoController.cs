using Fly01.Compras.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("servico")]
    public class ServicoController : ApiPlataformaController<Servico, ServicoBL>
    {
        public ServicoController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}