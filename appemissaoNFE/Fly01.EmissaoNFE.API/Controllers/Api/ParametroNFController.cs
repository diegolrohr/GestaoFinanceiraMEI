using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("parametronf")]
    public class ParametroNFController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(ParametroVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.ParametroNfBL.ValidaModel(entity);

                try
                {
                    Homologacao(entity);

                    Producao(entity);

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
            var sped = new SPEDCFGNFEProd.SPEDCFGNFE().CFGPARAMSPED(
                AppDefault.Token,
                entity.Producao,
                "N",
                entity.NumeroRetornoNF,
                entity.TipoAmbiente,
                entity.TipoModalidade,
                entity.VersaoNFe,
                entity.VersaoNFSe,
                entity.VersaoDPEC,
                "9.99",
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

            var cc = new SPEDCFGNFEProd.SPEDCFGNFE().CFGCCE(
                AppDefault.Token,
                entity.Producao,
                entity.TipoAmbiente,
                entity.TipoAmbiente,
                "1.00",
                "1.00",
                "1.00",
                "1.00",
                entity.HorarioVeraoRest,
                entity.TipoHorarioRest,
                "0",
                "0",
                "0",
                "0",
                "0",
                "0"
            );
        }

        public void Homologacao(ParametroVM entity)
        {
            var sped = new SPEDCFGNFE.SPEDCFGNFE().CFGPARAMSPED(
                AppDefault.Token,
                entity.Homologacao,
                "N",
                entity.NumeroRetornoNF,
                entity.TipoAmbiente,
                entity.TipoModalidade,
                entity.VersaoNFe,
                entity.VersaoNFSe,
                entity.VersaoDPEC,
                "9.99",
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

            var cc = new SPEDCFGNFE.SPEDCFGNFE().CFGCCE(
                AppDefault.Token,
                entity.Homologacao,
                entity.TipoAmbiente,
                entity.TipoAmbiente,
                "1.00",
                "1.00",
                "1.00",
                "1.00",
                entity.HorarioVeraoRest,
                entity.TipoHorarioRest,
                "0",
                "0",
                "0",
                "0",
                "0",
                "0"
            );
        }
    }
}