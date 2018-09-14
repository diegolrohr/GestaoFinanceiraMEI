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
                    EnviarParametrosNFe(entity);
                    EnviarParametrosNFSe(entity);

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

        public void ProducaoNFSe(ParametroVM entity)
        {
            //TODO confirmar com Machado
            var cfgNFS = new NFSE001Prod.NFSE001().CFGAMBNFSE001(
                AppDefault.Token,
                entity.Producao,
                entity.TipoAmbiente,
                "",//MODNFSE
                entity.VersaoNFSe,
                entity.CodigoIBGECidade,
                "",//CODSIAFI
                null,//uso
                null,//MAXLOTE
                null,//CNPJAUT
                null,//ENVSINC
                entity.UsuarioWebServer,
                Convert.FromBase64String(entity.SenhaWebServer),
                null, //AUTORIZACAO
                entity.ChaveAutenticacao,
                null, //TOKENID
                null, //CLIENTID
                null //CLIENTSECRET
            );
        }

        public void HomologacaoNFSe(ParametroVM entity)
        {
            //TODO confirmar com Machado
            var cfgNFS = new NFSE001.NFSE001().CFGAMBNFSE001(
                AppDefault.Token,
                entity.Homologacao,
                entity.TipoAmbiente,
                "",//MODNFSE
                entity.VersaoNFSe,
                entity.CodigoIBGECidade,
                "",//CODSIAFI
                null,//uso
                null,//MAXLOTE
                null,//CNPJAUT
                null,//ENVSINC
                entity.UsuarioWebServer,
                Convert.FromBase64String(entity.SenhaWebServer),
                null, //AUTORIZACAO
                entity.ChaveAutenticacao,
                null, //TOKENID
                null, //CLIENTID
                null //CLIENTSECRET
            );
        }
    }
}