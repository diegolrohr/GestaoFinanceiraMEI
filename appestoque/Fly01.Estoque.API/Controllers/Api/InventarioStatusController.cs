using Fly01.Estoque.Domain.Enums;
using Fly01.Core.Controllers.API;
using Fly01.Core.Helpers;
using System.Web.Http;

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