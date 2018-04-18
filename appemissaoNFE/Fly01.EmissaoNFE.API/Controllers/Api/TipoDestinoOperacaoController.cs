using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.API;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("tipodestinooperacao")]
    public class TipoDestinoOperacaoController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoDestinoOperacao)));
        }

    }
}