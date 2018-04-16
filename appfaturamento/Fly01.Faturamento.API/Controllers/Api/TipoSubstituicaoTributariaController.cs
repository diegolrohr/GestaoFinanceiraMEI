using System.Web.Http;
using Fly01.Core.API;
using Fly01.Compras.Domain.Enums;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("tiposubstituicaotributaria")]
    public class TipoSubstituicaoTributariaController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoSubstituicaoTributaria)));
        }

    }
}