using System.Web.Http;
using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.API;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("tipomensagemnfse")]
    public class TipoMensagemNFSEController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoMensagemNFSE)));
        }
    }
}