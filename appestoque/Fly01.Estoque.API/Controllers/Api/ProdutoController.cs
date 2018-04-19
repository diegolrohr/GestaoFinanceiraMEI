using Fly01.Estoque.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("produto")]
    public class ProdutoController : ApiPlataformaController<Produto, ProdutoBL>
    {
        public ProdutoController()
        {
            //se desativar, desativar nas chamadas que vao direto as bls
            MustProduceMessageServiceBus = true;
        }
    }
}