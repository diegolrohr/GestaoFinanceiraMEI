using System.Web.Http;
using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.Controllers.API;
using Fly01.Core.Helpers;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("tipoordemvenda")]
    public class TipoOrdemVendaController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoOrdemVenda)));
        }
    }
}