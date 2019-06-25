using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("empresa")]
    public class EmpresaController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(EmpresaVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.EmpresaBL.ValidaModel(entity);
                
                try
                {
                    var homolog = new SPEDADM.SPED_ENTIDADE
                    {
                        BAIRRO = entity.Bairro,
                        CEP = entity.Cep,
                        CPF = entity.Cpf == null ? "" : entity.Cpf,
                        CNPJ = entity.Cnpj == null ? "" : entity.Cnpj,
                        COD_MUN = entity.CodigoIBGECidade,
                        COD_PAIS = entity.CodigoPais == null ? "1058" : entity.CodigoPais,
                        COMPL = entity.Complemento,
                        DDD = entity.DDD,
                        EMAIL = entity.Email,
                        ENDERECO = entity.Endereco,
                        FANTASIA = entity.Fantasia,
                        FONE = entity.Fone,
                        ID_MATRIZ = null,
                        IDEMPRESA = null,
                        IE = entity.InscricaoEstadual,
                        IM = entity.InscricaoMunicipal,
                        MUN = entity.Municipio,
                        NIRE = entity.NIRE,
                        NUM = entity.Numero,
                        UF = entity.UF,
                        NOME = entity.Nome
                    };

                    var prod = new SPEDADMProd.SPED_ENTIDADE
                    {
                        BAIRRO = entity.Bairro,
                        CEP = entity.Cep,
                        CPF = entity.Cpf == null ? "" : entity.Cpf,
                        CNPJ = entity.Cnpj == null ? "" : entity.Cnpj,
                        COD_MUN = entity.CodigoIBGECidade,
                        COD_PAIS = entity.CodigoPais == null ? "1058" : entity.CodigoPais,
                        COMPL = entity.Complemento,
                        DDD = entity.DDD,
                        EMAIL = entity.Email,
                        ENDERECO = entity.Endereco,
                        FANTASIA = entity.Fantasia,
                        FONE = entity.Fone,
                        ID_MATRIZ = null,
                        IDEMPRESA = null,
                        IE = entity.InscricaoEstadual,
                        IM = entity.InscricaoMunicipal,
                        MUN = entity.Municipio,
                        NIRE = entity.NIRE,
                        NUM = entity.Numero,
                        UF = entity.UF,
                        NOME = entity.Nome
                    };

                    return Ok(new EntidadeRetornoVM
                    {
                        Homologacao = new SPEDADM.SPEDADM().ADMEMPRESAS(AppDefault.Token, homolog, new SPEDADM.SPED_ENTIDADEREFERENCIAL()),
                        Producao = new SPEDADMProd.SPEDADM().ADMEMPRESAS(AppDefault.Token, prod, new SPEDADMProd.SPED_ENTIDADEREFERENCIAL())
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