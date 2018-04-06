using Fly01.Core.API;
using Fly01.Faturamento.Domain.Enums;
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