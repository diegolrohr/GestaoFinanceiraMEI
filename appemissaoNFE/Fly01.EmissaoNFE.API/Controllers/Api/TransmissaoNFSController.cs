using Fly01.Core.API;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModelNfs;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("transmissaoNFS")]
    public class TransmissaoNFSController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(TransmissaoNFSVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.TransmissaoNFSBL.ValidaModel(entity);

                try
                {
                    var retorno = (int)entity.EntidadeAmbiente == 2 ? Homologacao(entity, unitOfWork) : Producao(entity, unitOfWork);

                    return Ok(retorno);
                }
                catch (Exception ex)
                {
                    if (unitOfWork.EntidadeBL.TSSException(ex))
                        unitOfWork.EntidadeBL.EmissaoNFeException(ex, entity);
                    
                    return InternalServerError(ex);
                }
            }
        }

        private object Producao(TransmissaoNFSVM entity, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }

        private object Homologacao(TransmissaoNFSVM entity, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }
    }
}