using Fly01.Core.API;
using Fly01.Estoque.Domain.Enums;
using System.Web.Http;

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