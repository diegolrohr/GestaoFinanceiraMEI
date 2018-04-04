using Fly01.Compras.Domain.Enums;
using Fly01.Core.Controllers.API;
using Fly01.Core.Helpers;
using System.Web.Http;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("tipoformapagamento")]
    public class TipoFormaPagamentoController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoFormaPagamento)));
        }
    }
}