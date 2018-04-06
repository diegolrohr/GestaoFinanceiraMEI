using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("regimetributario")]
    public class RegimeTributarioController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(ParametroVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.RegimeTributarioBL.ValidaModel(entity);

                try
                {
                    var parametro1 = new SPEDCFGNFE.PARAMNSE
                    {
                        INCCULT = entity.IncentivadorCultural ? "1" : "0",
                        REGTRIB = entity.TipoRegimeTributario,
                        SIMPNAC = entity.SimplesNacional ? "1" : "0"
                    };

                    var parametro2 = new SPEDCFGNFEProd.PARAMNSE
                    {
                        INCCULT = entity.IncentivadorCultural ? "1" : "0",
                        REGTRIB = entity.TipoRegimeTributario,
                        SIMPNAC = entity.SimplesNacional ? "1" : "0"
                    };

                    var homolog = new SPEDCFGNFE.SPEDCFGNFE().CFGPARAMNSE(AppDefault.Token, entity.Homologacao, parametro1);

                    var prod = new SPEDCFGNFEProd.SPEDCFGNFE().CFGPARAMNSE(AppDefault.Token, entity.Producao, parametro2);

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
        public IHttpActionResult Get(string entidade, TipoAmbiente tipoAmbiente)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.EntidadeBL.ValidaGet(entidade, tipoAmbiente);
                    
                try
                {
                    RegimeTributarioVM retorno;

                    if ((int)tipoAmbiente == 2)
                    {
                        var sped = new SPEDCFGNFE.SPEDCFGNFE().GETPARAMNSE(AppDefault.Token, entidade);

                        retorno = new RegimeTributarioVM
                        {
                            IncentivadorCultural = sped.INCCULT == "1" ? true : false,
                            TipoRegimeTributario = sped.REGTRIB,
                            SimplesNacional = sped.SIMPNAC == "1" ? true : false
                        };
                    }
                    else
                    {
                        var sped = new SPEDCFGNFEProd.SPEDCFGNFE().GETPARAMNSE(AppDefault.Token, entidade);

                        retorno = new RegimeTributarioVM
                        {
                            IncentivadorCultural = sped.INCCULT == "1" ? true : false,
                            TipoRegimeTributario = sped.REGTRIB,
                            SimplesNacional = sped.SIMPNAC == "1" ? true : false
                        };
                    }

                    return Ok(retorno);
                }
                catch (Exception ex)
                {
                    if (unitOfWork.EntidadeBL.TSSException(ex))
                    {
                        unitOfWork.EntidadeBL.EmissaoNFeException(ex, new EntidadeVM { });
                    }

                    return InternalServerError(ex);
                }
            }
        }
    }
}