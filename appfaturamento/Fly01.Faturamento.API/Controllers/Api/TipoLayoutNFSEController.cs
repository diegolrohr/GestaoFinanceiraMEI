using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("tipolayoutnfse")]
    public class TipoLayoutNFSEController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoLayoutNFSE)));
        }
    }
}