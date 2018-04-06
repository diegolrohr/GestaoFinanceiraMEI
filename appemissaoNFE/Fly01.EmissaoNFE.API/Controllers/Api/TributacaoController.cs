using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("tributacao")]
    public class TributacaoController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(Tributacao entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.TributacaoBL.ValidaModel(entity);

                return Ok(unitOfWork.TributacaoBL.GeraImpostos(entity));
            }
        }

        [HttpGet]
        public IHttpActionResult Get() {
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
