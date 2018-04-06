using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.Core.API;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    public class NFeController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(NFe entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(new { data = unitOfWork.NFeBL.ConvertToBase64(entity, Domain.Enums.CRT.SimplesNacional)});
            }
        }
    }
}