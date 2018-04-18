using Fly01.Compras.BL;
using Fly01.Compras.Domain.Entities;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("produto")]
    public class ProdutoController : ApiPlataformaController<Produto, ProdutoBL>
    {
        public ProdutoController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}