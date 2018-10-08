using Fly01.Core.API;
using Fly01.Core.Helpers;
using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using System;
using System.Web.Http;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
using System.Text;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("transmissaoNFS")]
    public class TransmissaoNFSController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(TransmissaoNFSVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.TransmissaoNFSBL.ValidaModel(entity);

                //TODO: aglutinar
                entity = unitOfWork.TransmissaoNFSBL.MontarValores(entity);
                entity.ItemTransmissaoNFSVM.AssinaturaHash = Assinatura.GeraAssinatura(entity.ItemTransmissaoNFSVM);
                unitOfWork.IbptNcmBL.CalculaImpostoIBPTNBS(entity);
                try
                {
                    var retorno = (int)entity.EntidadeAmbiente == 2 ? Homologacao(entity, unitOfWork) : Producao(entity, unitOfWork);

                    return Ok(retorno);
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

        private TransmissaoNFSRetornoVM Producao(TransmissaoNFSVM entity, UnitOfWork unitOfWork)
        {
            //Serializando a nota 
            var xmlUnicoTssString = unitOfWork.TransmissaoNFSBL.SerializeNotaNFS(entity);
            var xmlBase64 = Base64Helper.CodificaBase64(xmlUnicoTssString);

            //Validar base 64
            var notaSchema = new NFSE001Prod.NF
            {
                NOTAS = new NFSE001Prod.NF001[]
                {
                    new NFSE001Prod.NF001
                    {
                        ID = entity.ItemTransmissaoNFSVM.NotaId, //TODO: Confirmar com o Machado
                        XML = Convert.FromBase64String(xmlBase64)
                    }
                }
            };
            
            var validacao = new NFSE001Prod.NFSE001().SCHEMAX(AppDefault.Token, entity.Producao, entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE, notaSchema, false, false);
            var response = new TransmissaoNFSRetornoVM()
            {
                NotaId = entity.ItemTransmissaoNFSVM.NotaId.Replace(":", ""),
                XMLUnicoTSS = xmlUnicoTssString
            };

            if (validacao.Length > 0)
            {
                var schema = new SchemaXMLNFSRetornoVM
                {
                    Id = validacao[0].ID,
                    Mensagem = validacao[0].MENSAGEM.Replace("\\", "").Replace("\n", ""),
                    XML = Convert.ToBase64String(validacao[0].XML)
                };
                response.Error = schema;
            }
            else
            {
                var NFSE = new NFSE001Prod.NFSE()
                {
                    NOTAS = new NFSE001Prod.NFSES1[] {
                        new NFSE001Prod.NFSES1()
                        {
                            ID = entity.ItemTransmissaoNFSVM.NotaId,
                            CODMUN = entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE,
                            XML = Convert.FromBase64String(xmlBase64)
                        }
                    }
                };

                new NFSE001Prod.NFSE001().REMESSANFSE001(
                    AppDefault.Token,
                    entity.Producao,
                    NFSE,
                    entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE,
                    true,
                    true,
                    false,
                    false
                );
            }

            return response;
        }

        private TransmissaoNFSRetornoVM Homologacao(TransmissaoNFSVM entity, UnitOfWork unitOfWork)
        {
            //Serializando a nota 
            var xmlUnicoTssString = unitOfWork.TransmissaoNFSBL.SerializeNotaNFS(entity);
            var xmlBase64 = Base64Helper.CodificaBase64(xmlUnicoTssString);

            //Validar base 64
            var notaSchema = new NFSE001.NF
            {
                NOTAS = new NFSE001.NF001[]
                {
                    new NFSE001.NF001
                    {
                        ID = entity.ItemTransmissaoNFSVM.NotaId, 
                        XML = Convert.FromBase64String(xmlBase64)
                    }
                }
            };

            var validacao = new NFSE001.NFSE001().SCHEMAX(AppDefault.Token, entity.Homologacao, entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE, notaSchema, false, false);
            var response = new TransmissaoNFSRetornoVM()
            {
                NotaId = entity.ItemTransmissaoNFSVM.NotaId.Replace(":", ""),
                XMLUnicoTSS = xmlUnicoTssString
            };

            if (!string.IsNullOrEmpty(validacao[0].MENSAGEM))
            {
                var schema = new SchemaXMLNFSRetornoVM
                {
                    Id = validacao[0].ID,
                    Mensagem = validacao[0].MENSAGEM.Replace("\\", "").Replace("\n", ""),
                    XML = Encoding.UTF8.GetString(validacao[0].XML)
                };
                response.Error = schema;
            }
            else
            {
                var NFSE = new NFSE001.NFSE()
                {
                    NOTAS = new NFSE001.NFSES1[] {
                        new NFSE001.NFSES1()
                        {
                            ID = entity.ItemTransmissaoNFSVM.NotaId,
                            CODMUN = entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE,
                            XML = Convert.FromBase64String(xmlBase64)
                        }
                    }
                };

                var t = new NFSE001.NFSE001().REMESSANFSE001(
                    AppDefault.Token,
                    entity.Homologacao,
                    NFSE,
                    entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE,
                    true,
                    true,
                    false,
                    false
                );
            }

            return response;
        }
    }
}