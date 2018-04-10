using Fly01.Core.API;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("chave")]
    public class ChaveController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(ChaveVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.ChaveBL.ValidaModel(entity);
                
                return Ok(unitOfWork.ChaveBL.ChaveNFe(entity));
            }
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
 