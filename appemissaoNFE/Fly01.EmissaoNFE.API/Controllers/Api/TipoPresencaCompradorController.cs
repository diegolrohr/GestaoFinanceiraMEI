using Fly01.Core.API;
using System.Web.Http;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Enum;

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