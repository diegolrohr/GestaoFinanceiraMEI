using Fly01.Compras.BL;
using System.Web.OData.Routing;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.API.Controllers.Api
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