using Fly01.Estoque.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("tipomovimento")]
    public class TipoMovimentoController : ApiPlataformaController<TipoMovimento, TipoMovimentoBL>
    {
        public TipoMovimentoController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}