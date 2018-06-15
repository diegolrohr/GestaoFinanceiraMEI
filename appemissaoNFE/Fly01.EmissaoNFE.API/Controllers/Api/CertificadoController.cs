using System;
using Fly01.Core.API;
using System.Web.Http;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.API.Model;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.ViewModel;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("certificado")]
    public class CertificadoController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(CertificadoVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.CertificadoBL.ValidaModel(entity);
                
                try
                {
                    var homolog = new SPEDCFGNFE.SPEDCFGNFE().CFGCERTIFICATEPFX(
                        AppDefault.Token, 
                        entity.Homologacao, 
                        Convert.FromBase64String(entity.Certificado), 
                        Convert.FromBase64String(entity.Senha)
                    );

                    //TODO Diego ver tss prod local
                    var prod = new SPEDCFGNFEProd.SPEDCFGNFE().CFGCERTIFICATEPFX(
                        AppDefault.Token,
                        entity.Producao,
                        Convert.FromBase64String(entity.Certificado),
                        Convert.FromBase64String(entity.Senha)
                    );

                    var dados = new SPEDCFGNFEProd.SPEDCFGNFE().CFGSTATUSCERTIFICATE(AppDefault.Token, entity.Producao, "");
                    //var dados = new SPEDCFGNFE.SPEDCFGNFE().CFGSTATUSCERTIFICATE(AppDefault.Token, entity.Homologacao, "");

                    var response = new CertificadoRetornoVM
                    {
                        Tipo = dados[0].CERTIFICATETYPE,
                        Emissor = Convert.ToBase64String(dados[0].ISSUER),
                        Pessoa = dados[0].SUBJECT,
                        DataEmissao = dados[0].VALIDFROM,
                        DataExpiracao = dados[0].VALIDTO,
                        Versao = dados[0].VERSION
                    };

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

        [HttpGet]
        public IHttpActionResult Get(string entidade, TipoAmbiente tipoAmbiente)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.EntidadeBL.ValidaGet(entidade, tipoAmbiente);
                     
                try
                {
                    CertificadoRetornoVM certificado;

                    if ((int)tipoAmbiente == 2)
                    {
                        var sped = new SPEDCFGNFE.SPEDCFGNFE().CFGSTATUSCERTIFICATE(AppDefault.Token, entidade, "");

                        certificado = new CertificadoRetornoVM
                        {
                            Tipo = sped[0].CERTIFICATETYPE,
                            Emissor = Convert.ToBase64String(sped[0].ISSUER),
                            Pessoa = sped[0].SUBJECT,
                            DataEmissao = sped[0].VALIDFROM,
                            DataExpiracao = sped[0].VALIDTO,
                            Versao = sped[0].VERSION
                        };
                    }
                    else
                    {
                        var sped = new SPEDCFGNFEProd.SPEDCFGNFE().CFGSTATUSCERTIFICATE(AppDefault.Token, entidade, "");

                        certificado = new CertificadoRetornoVM
                        {
                            Tipo = sped[0].CERTIFICATETYPE,
                            Emissor = Convert.ToBase64String(sped[0].ISSUER),
                            Pessoa = sped[0].SUBJECT,
                            DataEmissao = sped[0].VALIDFROM,
                            DataExpiracao = sped[0].VALIDTO,
                            Versao = sped[0].VERSION
                        };
                    }

                    return Ok( certificado );
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