using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using System.Web.Http;
using System.Linq;
using System.Web.OData;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("categoria")]
    public class CategoriaController : ApiEmpresaController<Categoria, CategoriaBL>
    {
        public CategoriaController()
        {            
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }
    }
}