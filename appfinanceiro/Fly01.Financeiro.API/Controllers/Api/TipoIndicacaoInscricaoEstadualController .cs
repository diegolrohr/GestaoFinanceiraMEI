using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Helpers;
using Fly01.Financeiro.Domain.Enums;

namespace Fly01.Financeiro.API.Controllers.Api
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