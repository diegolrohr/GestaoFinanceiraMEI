using System.Web.Http;
using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.API;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("tipoTributacaoISS")]
    public class TipoTributacaoISSController : ApiBaseController
    {
         public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoTributacaoISS)));
        }
    }
}