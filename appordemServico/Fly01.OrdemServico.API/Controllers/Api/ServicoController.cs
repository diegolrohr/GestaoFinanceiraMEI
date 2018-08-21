using Fly01.Core.Entities.Domains.Commons;
using Fly01.OrdemServico.BL;
using System.Web.OData.Routing;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [ODataRoutePrefix("servico")]
    public class ServicoController : ApiPlataformaController<Servico, ServicoBL>
    {
        public ServicoController()
        {
            //se desativar, desativar nas chamadas que vao direto as bls
            MustProduceMessageServiceBus = true;
        }
    }
}
