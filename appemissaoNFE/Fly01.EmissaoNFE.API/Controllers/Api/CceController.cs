using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Controllers.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("cce")]
    public class CceController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(CceVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.CceBL.ValidaModel(entity);

                try
                {
                    if((int)entity.EntidadeAmbiente == 2)
                    {
                        Homologacao(entity);
                    }
                    else
                    {
                        Producao(entity);
                    }

                    return Ok(new { success = true });
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

        public void Producao(CceVM entity)
        {
            var sped = new SPEDCFGNFEProd.SPEDCFGNFE().CFGCCE(
                AppDefault.Token,
                entity.Producao,
                entity.TipoAmbienteCCE,
                entity.TipoAmbienteEPP,
                entity.VersaoLayoutCCE,
                entity.VersaoLayoutEventoCCE,
                entity.VersaoEventoCCE,
                entity.VersaoCCE,
                entity.HorarioDeVerao ? "1" : "2",
                entity.TipoFusoHorario.ToString(),
                entity.VersaoLayoutEPP,
                entity.VersaoLayoutEventoEPP,
                entity.VersaoEventoEPP,
                entity.VersaoEPP,
                null,
                null
            );
        }

        public void Homologacao(CceVM entity)
        {
            var sped = new SPEDCFGNFE.SPEDCFGNFE().CFGCCE(
                AppDefault.Token,
                entity.Homologacao,
                entity.TipoAmbienteCCE,
                entity.TipoAmbienteEPP,
                entity.VersaoLayoutCCE,
                entity.VersaoLayoutEventoCCE,
                entity.VersaoEventoCCE,
                entity.VersaoCCE,
                entity.HorarioDeVerao ? "1" : "2",
                entity.TipoFusoHorario.ToString(),
                entity.VersaoLayoutEPP,
                entity.VersaoLayoutEventoEPP,
                entity.VersaoEventoEPP,
                entity.VersaoEPP,
                null,
                null
            );
        }
    }
}