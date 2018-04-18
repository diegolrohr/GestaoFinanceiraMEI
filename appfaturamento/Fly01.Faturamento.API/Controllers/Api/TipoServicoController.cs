using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

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