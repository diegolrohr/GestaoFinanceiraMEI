using Fly01.Core.API;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Estoque.API.Controllers.Api
{
    [RoutePrefix("inventariostatus")]
    public class InventarioStatusController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(InventarioStatus)));
        }
    }
}