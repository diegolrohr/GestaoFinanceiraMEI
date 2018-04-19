using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
using System.Web.Http;
using Fly01.Core.Helpers;

namespace Fly01.Estoque.API.Controllers.Api
{
    [RoutePrefix("tipoproduto")]
    public class TipoProdutoController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoProduto)));
        }
    }
}