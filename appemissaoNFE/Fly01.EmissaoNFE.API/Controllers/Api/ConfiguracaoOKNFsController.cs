using System;
using Fly01.Core.API;
using System.Web.Http;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("configuracaoOKNFS")]
    public class ConfiguracaoOKNFSController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post()
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

        [HttpGet]
        public IHttpActionResult Get(string entidade, TipoAmbiente tipoAmbiente, string codigoMunicipio)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.EntidadeBL.ValidaGet(entidade, tipoAmbiente);
                unitOfWork.CidadeBL.ValidaCodigoIBGEException(codigoMunicipio);

                var sped = string.Empty;
                try
                {
                    if (tipoAmbiente == TipoAmbiente.Homologacao)
                        new NFSE001.NFSE001().CFGREADYX(AppDefault.Token, entidade, codigoMunicipio);
                    else
                        new NFSE001Prod.NFSE001().CFGREADYX(AppDefault.Token, entidade, codigoMunicipio);
                   
                    return Ok(new { success = true });
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