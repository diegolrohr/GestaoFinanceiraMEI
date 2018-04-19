using Fly01.Estoque.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("grupoproduto")]
    public class GrupoProdutoController : ApiPlataformaController<GrupoProduto, GrupoProdutoBL>
    {
        public GrupoProdutoController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}