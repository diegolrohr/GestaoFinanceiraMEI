using System.Web.OData.Routing;
using System.Web.Http;
using System.Linq;
using System.Web.OData;
using Fly01.Faturamento.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("categoria")]
    public class CategoriaController : ApiPlataformaController<Categoria, CategoriaBL>
    {
        public CategoriaController()
        {
            MustProduceMessageServiceBus = true;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }
    }
}