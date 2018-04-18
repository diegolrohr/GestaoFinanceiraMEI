using System.Web.OData.Routing;
using Fly01.Faturamento.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
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