using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.Core.API;
using System.Web.Http;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    public class NFeController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(NFeVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var base64 = Base64Helper.CodificaBase64(unitOfWork.NFeBL.ConvertToXML(entity, entity.InfoNFe.Emitente.CRT));
                return Ok(new { data = base64});
            }
        }
    }
}