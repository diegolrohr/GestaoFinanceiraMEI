using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Controllers.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("empresaId")]
    public class EmpresaIdController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(EmpresaVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.EmpresaIdBL.ValidaModel(entity);

                try
                {
                    return Ok(new EntidadeRetornoVM
                    {
                        Homologacao = new SPEDADM.SPEDADM().GETADMEMPRESASID(
                            AppDefault.Token,
                            entity.Cnpj == null ? "" : entity.Cnpj,
                            entity.Cpf == null ? "" : entity.Cpf,
                            entity.InscricaoEstadual,
                            entity.UF,
                            null
                        ),
                        Producao = new SPEDADMProd.SPEDADM().GETADMEMPRESASID(
                            AppDefault.Token,
                            entity.Cnpj == null ? "" : entity.Cnpj,
                            entity.Cpf == null ? "" : entity.Cpf,
                            entity.InscricaoEstadual,
                            entity.UF,
                            null
                        )
                    });

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
    }
}