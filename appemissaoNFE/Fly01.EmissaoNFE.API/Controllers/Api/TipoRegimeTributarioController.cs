using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.API;
using System.Web.Http;
using Fly01.Core.Helpers;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("tiporegimetributario")]
    public class TipoRegimeTributarioController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoRegimeTributario)));
        }
    }
}