using System;
using Fly01.Core.API;
using System.Web.Http;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("configuracaoOK")]
    public class ConfiguracaoOKController : ApiBaseController
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
        public IHttpActionResult Get(string entidade, TipoAmbiente tipoAmbiente)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.EntidadeBL.ValidaGet(entidade, tipoAmbiente);

                try
                {
                    if ((int)tipoAmbiente == 2)
                    {
                        var sped = new SPEDCFGNFE.SPEDCFGNFE().CFGREADYEX(AppDefault.Token, entidade);
                    }
                    else
                    {
                        var sped = new SPEDCFGNFEProd.SPEDCFGNFE().CFGREADYEX(AppDefault.Token, entidade);
                    }

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