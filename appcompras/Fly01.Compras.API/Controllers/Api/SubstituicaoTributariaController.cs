using System.Web.OData.Routing;
using Fly01.Compras.BL;
using Fly01.Compras.Domain.Entities;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("substituicaotributaria")]
    public class SubstituicaoTributariaController : ApiPlataformaController<SubstituicaoTributaria, SubstituicaoTributariaBL>
    {
        public SubstituicaoTributariaController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}