using System.Web.Http;
using Fly01.Estoque.Domain.Enums;
using Fly01.Core.API;

namespace Fly01.Estoque.API.Controllers.Api
{
    [RoutePrefix("tipoentradasaida")]
    public class TipoEntradaSaidaController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoEntradaSaida)));
        }
    }
}