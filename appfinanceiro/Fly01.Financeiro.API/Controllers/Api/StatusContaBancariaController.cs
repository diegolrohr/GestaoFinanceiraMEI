using System.Web.Http;
using Fly01.Core.Controllers.API;
using Fly01.Core.Helpers;
using Fly01.Financeiro.Domain.Enums;

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