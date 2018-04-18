using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("statuscontabancaria")]
    public class StatusContaBancariaController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(StatusContaBancaria)));
        }
    }
}