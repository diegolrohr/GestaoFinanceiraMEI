using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Web.Http;
using Fly01.Core.Helpers;
using System.Threading.Tasks;

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
                unitOfWork.SiafiBL.RetornaSiafi(entity);
                unitOfWork.ParametroNfBL.ValidaModel(entity);

                try
                {
                    Parallel.Invoke(() =>
                    {
                        EnviarParametrosNFe(entity);
                    },() =>
                    {
                        EnviarParametrosNFSe(entity);
                    });

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

        private void EnviarParametrosNFSe(ParametroVM entity)
        {
            HomologacaoNFSe(entity);
            ProducaoNFSe(entity);
        }

        private void EnviarParametrosNFe(ParametroVM entity)
        {
            HomologacaoNFe(entity);
            ProducaoNFe(entity);
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

        public void ProducaoNFe(ParametroVM entity)
        {
            var sped = new SPEDCFGNFEProd.SPEDCFGNFE().CFGPARAMSPED(
                AppDefault.Token,
                entity.Producao,
                "N",
                entity.NumeroRetornoNF,
                entity.TipoAmbiente,
                entity.TipoModalidade,
                entity.VersaoNFe,
                "0.00",
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

        public void HomologacaoNFe(ParametroVM entity)
        {
            var sped = new SPEDCFGNFE.SPEDCFGNFE().CFGPARAMSPED(
                AppDefault.Token,
                entity.Homologacao,
                "N",
                entity.NumeroRetornoNF,
                entity.TipoAmbiente,
                entity.TipoModalidade,
                entity.VersaoNFe,
                "0.00",
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

        public void ProducaoNFSe(ParametroVM entity)
        {
            var AutorizacaoEncode = Base64Helper.CodificaBase64(entity.Autorizacao);

            var cfgNFS = new NFSE001Prod.NFSE001().CFGAMBNFSE001(
                AppDefault.Token,
                entity.Producao,
                entity.TipoAmbienteNFS,
                "0",
                entity.VersaoNFSe,
                entity.CodigoIBGECidade,
                entity.Siafi,
                null,
                "1",
                null,
                null,
                entity.UsuarioWebServer,
                Convert.FromBase64String(entity.SenhaWebServer),
                Convert.FromBase64String(AutorizacaoEncode),
                entity.ChaveAutenticacao,
                null,
                null,
                null
            );
        }

        public void HomologacaoNFSe(ParametroVM entity)
        {
            var AutorizacaoEncode = Base64Helper.CodificaBase64(entity.Autorizacao);

            var cfgNFS = new NFSE001.NFSE001().CFGAMBNFSE001(
                AppDefault.Token,
                entity.Homologacao,
                entity.TipoAmbienteNFS,
                "0",
                entity.VersaoNFSe,
                entity.CodigoIBGECidade,
                entity.Siafi,
                null,
                "1",
                null,
                null,
                entity.UsuarioWebServer,
                Convert.FromBase64String(entity.SenhaWebServer),
                Convert.FromBase64String(AutorizacaoEncode),
                entity.ChaveAutenticacao,
                null,
                null,
                null
            );
        }
    }
}