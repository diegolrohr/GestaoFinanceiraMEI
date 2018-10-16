using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using System.Linq;
using Fly01.Core.Notifications;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("cancelarNFS")]
    public class CancelarNFSController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(CancelarNFSVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.CancelarNFSBL.ValidaModel(entity);

                try
                {
                    var response = entity.EntidadeAmbiente == TipoAmbiente.Homologacao ? Homologacao(entity) : Producao(entity);

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

        public CancelarNFSRetornoVM Producao(CancelarNFSVM entity)
        {
            var response = new CancelarNFSRetornoVM();

            var municipiosHomologados = new NFSE001Prod.NFSE001().RETMUNCANC(
                    AppDefault.Token
                );

            var CodigosIBGE = municipiosHomologados.Split('-').ToList();
            if (CodigosIBGE.Any(x => x.Equals(entity.CodigoIBGE.ToUpper(), StringComparison.InvariantCultureIgnoreCase)))
            {
                var base64 = Base64Helper.CodificaBase64(entity.XMLUnicoTSSString);

                var NFSE = new NFSE001Prod.NFSE()
                {
                    NOTAS = new NFSE001Prod.NFSES1[]
                    {
                        new NFSE001Prod.NFSES1()
                        {
                            ID = entity.IdNotaFiscal,
                            CODMUN = entity.CodigoIBGE,
                            CODCANC = "",
                            XML = Convert.FromBase64String(base64)//xml unico
                        }
                    }
                };

                var cancelamento = new NFSE001Prod.NFSE001().CANCELANFSE001(
                    AppDefault.Token,
                    entity.Producao,
                    NFSE,
                    entity.CodigoIBGE
                );

                if (cancelamento.ID.Length > 0)
                {
                    response.Nota = cancelamento.ID[0];
                }
                else
                {
                    throw new BusinessException(string.Format("Cancelamento solicitado sem retorno do TSS"));
                }
            }
            else
            {
                throw new BusinessException(string.Format("Cancelamento para o município ({0}) não esta homologado via TSS, somente via portal da prefeitura.", entity.CodigoIBGE));
            }

            return response;
        }

        public CancelarNFSRetornoVM Homologacao(CancelarNFSVM entity)
        {
            var response = new CancelarNFSRetornoVM();
            var municipiosHomologados = new NFSE001Prod.NFSE001().RETMUNCANC(
                    AppDefault.Token
                );

            var CodigosIBGE = municipiosHomologados.Split('-').ToList();
            if (CodigosIBGE.Any(x => x.Equals(entity.CodigoIBGE.ToUpper(), StringComparison.InvariantCultureIgnoreCase)))
            {
                var base64 = Base64Helper.CodificaBase64(entity.XMLUnicoTSSString);

                var NFSE = new NFSE001.NFSE()
                {
                    NOTAS = new NFSE001.NFSES1[]
                    {
                        new NFSE001.NFSES1()
                        {
                            ID = entity.IdNotaFiscal,
                            CODMUN = entity.CodigoIBGE,
                            CODCANC = "",
                            XML = Convert.FromBase64String(base64)
                        }
                    }
                };

                var cancelamento = new NFSE001.NFSE001().CANCELANFSE001(
                    AppDefault.Token,
                    entity.Homologacao,
                    NFSE,
                    entity.CodigoIBGE
                );

                if (cancelamento.ID.Length > 0)
                {
                    response.Nota = cancelamento.ID[0];
                }
                else
                {
                    throw new BusinessException(string.Format("Cancelamento solicitado sem retorno do TSS"));
                }
            }
            else
            {
                throw new BusinessException(string.Format("Cancelamento para o município ({0}) não esta homologado via TSS, somente via portal da prefeitura.", entity.CodigoIBGE));
            }

            return response;
        }
    }
}
