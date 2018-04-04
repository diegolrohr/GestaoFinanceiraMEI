using System.Web.Http;
using Fly01.Core.Controllers.API;
using Fly01.Core.Helpers;
using Fly01.Faturamento.Domain.Enums;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("tipoindicacaoinscricaoestadual")]
    public class TipoIndicacaoInscricaoEstadualController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoIndicacaoInscricaoEstadual)));
        }
    }
}