using Fly01.Compras.BL;
using Fly01.Compras.Domain.Entities;
using System.Web.OData.Routing;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("condicaoparcelamento")]
    public class CondicaoParcelamentoController : ApiPlataformaController<CondicaoParcelamento, CondicaoParcelamentoBL>
    {
        public CondicaoParcelamentoController()
        {
            MustProduceMessageServiceBus = true;
        }     
    }
}