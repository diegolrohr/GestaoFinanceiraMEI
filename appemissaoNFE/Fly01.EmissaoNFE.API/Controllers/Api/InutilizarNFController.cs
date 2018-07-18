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
    [RoutePrefix("inutilizarnf")]
    public class InutilizarNFController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(InutilizarNFVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.InutilizarNFBL.ValidaModel(entity);

                try
                {
                    var sefazChaveAcesso = unitOfWork.ChaveBL.GeraChave(
                                    entity.EmpresaCodigoUF.ToString(),
                                    DateTime.Now.Year.ToString(),
                                    DateTime.Now.Month.ToString(),
                                    entity.EmpresaCnpj,
                                    entity.ModeloDocumentoFiscal.ToString(),
                                    entity.Serie.ToString(),
                                    entity.Numero.ToString(),
                                    ((int)TipoModalidade.Normal).ToString(),
                                    entity.Numero.ToString()
                                );
                    sefazChaveAcesso = sefazChaveAcesso.Replace("NFe", "");
                    var idBase64 = Base64Helper.CodificaBase64("Id=\"" + sefazChaveAcesso+"\"");


                    var response = entity.EntidadeAmbiente == TipoAmbiente.Homologacao ?
                            Homologacao(entity.Homologacao, sefazChaveAcesso, idBase64) :
                            Producao(entity.Producao, sefazChaveAcesso, idBase64);

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

        public InutilizarNFRetornoVM Producao(string entidade, string sefazChaveAcesso, string idBase64)
        {
            var inutilizarNota = new NFESBRAProd.NFE();
            inutilizarNota.NOTAS = new NFESBRAProd.NFES[]{
                new NFESBRAProd.NFES
                {
                    ID = sefazChaveAcesso,
                    MAIL = "",
                    XML = Convert.FromBase64String(idBase64)
                }
            };

            var retorno = new NFESBRAProd.NFESBRA().CANCELANOTAS(
                AppDefault.Token,
                entidade,
                inutilizarNota,
                null
            );

            return new InutilizarNFRetornoVM()
            {
                SefazChaveAcesso = sefazChaveAcesso
            };
        }

        public InutilizarNFRetornoVM Homologacao(string entidade, string sefazChaveAcesso, string idBase64)
        {
            var inutilizarNota = new NFESBRA.NFE();
            inutilizarNota.NOTAS = new NFESBRA.NFES[]{
                new NFESBRA.NFES
                {
                    ID = sefazChaveAcesso,
                    MAIL = "",
                    XML = Convert.FromBase64String(idBase64)
                }
            };

            var retorno = new NFESBRA.NFESBRA().CANCELANOTAS(
                AppDefault.Token,
                entidade,
                inutilizarNota,
                null
            );

            return new InutilizarNFRetornoVM()
            {
                SefazChaveAcesso = sefazChaveAcesso
            };
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
