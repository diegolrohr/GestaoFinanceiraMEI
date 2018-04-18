using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("statusconciliado")]
    public class StatusConciliadoController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(StatusConciliado)));
        }
    }
}