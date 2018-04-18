using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
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