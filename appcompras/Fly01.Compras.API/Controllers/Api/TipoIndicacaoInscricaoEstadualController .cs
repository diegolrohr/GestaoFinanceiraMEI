using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.API.Controllers.Api
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