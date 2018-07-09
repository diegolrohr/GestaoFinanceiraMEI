using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("cartacorrecao")]
    public class CartaCorrecaoController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(CartaCorrecaoVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.CartaCorrecaoBL.ValidaModel(entity);

                try
                {
                    var response = entity.EntidadeAmbiente == TipoAmbiente.Homologacao ? Homologacao(entity, unitOfWork) : Producao(entity, unitOfWork);

                    return Ok(response);

                }
                catch (Exception ex)
                {
                    if (unitOfWork.EntidadeBL.TSSException(ex))
                    {
                        unitOfWork.EntidadeBL.EmissaoNFeException(ex, entity);
                    }

                    return InternalServerError(ex);
                }
            }
        }        

        public CartaCorrecaoRetornoVM Producao(CartaCorrecaoVM entity, UnitOfWork unitOfWork)
        {
            var base64 = unitOfWork.CartaCorrecaoBL.Serialize(entity);
            var retorno = new CartaCorrecaoRetornoVM();

            //var cartaCorrecaoEvento = new NFESBRAProd.NFESBRA().NFEREMESSAEVENTO(
            //    AppDefault.Token,
            //    entity.Producao,
            //    Convert.FromBase64String(base64)
            //);

            //if(cartaCorrecaoEvento != null)
            //{
            //    retorno.IdEvento = "ID"+
            //}

            return retorno;
        }

        public CartaCorrecaoRetornoVM Homologacao(CartaCorrecaoVM entity, UnitOfWork unitOfWork)
        {
            var base64 = unitOfWork.CartaCorrecaoBL.Serialize(entity);
            var retorno = new CartaCorrecaoRetornoVM();

            //var cartaCorrecaoEvento = new NFESBRA.NFESBRA().NFEREMESSAEVENTO(
            //    AppDefault.Token,
            //    entity.Homologacao,
            //    Convert.FromBase64String(base64)
            //);

            //if(cartaCorrecaoEvento != null)
            //{
            //    retorno.IdEvento = "ID"+
            //}

            return retorno;
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
