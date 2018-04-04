using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Controllers.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("parametro")]
    public class ParametroController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(ParametroVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.RegimeTributarioBL.ValidaModel(entity);
                unitOfWork.ParametroNfBL.ValidaModel(entity);

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

        public void Producao(ParametroVM entity)
        {
            var parametros = new SPEDCFGNFEProd.PARAMNSE
            {
                INCCULT = entity.IncentivadorCultural ? "1" : "0",
                REGTRIB = entity.TipoRegimeTributario,
                SIMPNAC = entity.SimplesNacional ? "1" : "0"
            };

            var sped = new SPEDCFGNFEProd.SPEDCFGNFE().CFGPARAMNSE(AppDefault.Token, entity.Producao, parametros);

            sped = new SPEDCFGNFEProd.SPEDCFGNFE().CFGPARAMSPED(
                AppDefault.Token,
                entity.Producao,
                "N",
                entity.NumeroRetornoNF,
                entity.TipoAmbiente,
                entity.TipoModalidade,
                entity.VersaoNFe,
                entity.VersaoNFSe,
                entity.VersaoDPEC,
                "",
                "",
                new byte[1],
                "",
                "",
                "",
                "",
                null,
                true,
                true,
                entity.EnviaDanfe ? "1" : "0",
                entity.UsaEPEC ? "1" : "0"
            );
        }

        public void Homologacao(ParametroVM entity)
        {
            var parametros = new SPEDCFGNFE.PARAMNSE
            {
                INCCULT = entity.IncentivadorCultural ? "1" : "0",
                REGTRIB = entity.TipoRegimeTributario,
                SIMPNAC = entity.SimplesNacional ? "1" : "0"
            };

            var sped = new SPEDCFGNFE.SPEDCFGNFE().CFGPARAMNSE(AppDefault.Token, entity.Homologacao, parametros);

            sped = new SPEDCFGNFE.SPEDCFGNFE().CFGPARAMSPED(
                AppDefault.Token,
                entity.Homologacao,
                "N",
                entity.NumeroRetornoNF,
                entity.TipoAmbiente,
                entity.TipoModalidade,
                entity.VersaoNFe,
                entity.VersaoNFSe,
                entity.VersaoDPEC,
                "",
                "",
                new byte[1],
                "",
                "",
                "",
                "",
                null,
                true,
                true,
                entity.EnviaDanfe ? "1" : "0",
                entity.UsaEPEC ? "1" : "0"
            );
        }        
    }
}