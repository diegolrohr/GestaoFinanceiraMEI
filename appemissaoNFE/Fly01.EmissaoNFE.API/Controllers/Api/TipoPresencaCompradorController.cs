using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.API;
using Fly01.Core.Helpers;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("tipopresencacomprador")]
    public class TipoPresencaCompradorController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoPresencaComprador)));
        }
    }
}