using System.Web.Http;
using Fly01.Core.Helpers;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("tipocarteira")]
    public class TipoCarteiraController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoCarteira)));
        }
    }
}