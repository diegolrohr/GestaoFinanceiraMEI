using System.Web.Http;
using Fly01.Core.Controllers.API;
using Fly01.Core.Helpers;
using Fly01.Compras.Domain.Enums;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("tiposimulacao")]
    public class TipoSimulacaoController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoSimulacao)));
        }
    }
}