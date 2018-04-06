using System.Web.Http;
using Fly01.Financeiro.Domain.Enums;
using Fly01.Core.API;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("tiposimulacao")]
    public class TipoSimulacaoController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoSimulacao)));
        }
    }
}