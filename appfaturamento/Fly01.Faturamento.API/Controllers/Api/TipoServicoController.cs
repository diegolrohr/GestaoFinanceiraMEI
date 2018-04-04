using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.Controllers.API;
using Fly01.Core.Helpers;
using System.Web.Http;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("tipoServico")]
    public class TipoServicoController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoServico)));
        }
    }
}