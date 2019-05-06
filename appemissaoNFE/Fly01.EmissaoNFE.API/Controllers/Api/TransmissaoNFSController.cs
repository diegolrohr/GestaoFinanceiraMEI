using Fly01.Core.API;
using Fly01.Core.Helpers;
using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using System;
using System.Web.Http;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
using System.Text;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("transmissaoNFS")]
    public class TransmissaoNFSController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(TransmissaoNFSVM entity)
        {
            entity?.TrimAllStrings();

            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.TransmissaoNFSBL.ValidaModel(entity);

                unitOfWork.TransmissaoNFSBL.AglutinarServicos(entity);
                unitOfWork.TransmissaoNFSBL.MontarValores(entity);

                entity.ItemTransmissaoNFSVM.AssinaturaHash = Assinatura.GeraAssinatura(entity.ItemTransmissaoNFSVM);
                unitOfWork.IbptNcmBL.CalculaImpostoIBPTNBS(entity);
                /// <summary>
                /// 3547809	Santo André SP não deve sair o CNAE
                /// </summary>
                if(entity.ItemTransmissaoNFSVM.Identificacao.CodigoIBGEPrestador == "3547809")
                {
                    entity.ItemTransmissaoNFSVM.Atividade.CodigoCNAE = string.Empty;
                    foreach (var item in entity.ItemTransmissaoNFSVM.Servicos)
                    {
                        item.CNAE = string.Empty;
                    }
                }

                try
                {
                    var retorno = entity.EntidadeAmbienteNFS == TipoAmbiente.Homologacao ? Homologacao(entity, unitOfWork) : Producao(entity, unitOfWork);

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
            var xmlUnicoTssString = unitOfWork.TransmissaoNFSBL.SerializeNotaNFS(entity);
            var xmlBase64 = Base64Helper.CodificaBase64(xmlUnicoTssString);

            //Validar base 64
            var notaSchema = new NFSE001Prod.NF
            {
                NOTAS = new NFSE001Prod.NF001[]
                {
                    new NFSE001Prod.NF001
                    {
                        ID = entity.ItemTransmissaoNFSVM.NotaId,
                        XML = Convert.FromBase64String(xmlBase64)
                    }
                }
            };

            var response = new TransmissaoNFSRetornoVM()
            {
                NotaId = entity.ItemTransmissaoNFSVM.NotaId,
                XMLUnicoTSS = xmlUnicoTssString
            };

            ///Essa prefeitura não tem schema, o schema é validado pela prefeitura..
            if (entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE != "4104204")
            {
                var validacao = new NFSE001Prod.NFSE001().SCHEMAX(AppDefault.Token, entity.Producao, entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE, notaSchema, false, false);
                response.XMLGerado = Encoding.UTF8.GetString(validacao[0]?.XML);

                if (!string.IsNullOrEmpty(validacao[0]?.MENSAGEM) && validacao[0]?.MENSAGEM.Trim().Length > 0)
                {
                    var schema = new SchemaXMLNFSRetornoVM
                    {
                        Id = validacao[0].ID,
                        Mensagem = validacao[0].MENSAGEM.Trim().Replace("\\", "").Replace("\n", "")
                    };
                    response.Error = schema;
                    return response;
                }
            }

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

            return response;
        }

        private TransmissaoNFSRetornoVM Homologacao(TransmissaoNFSVM entity, UnitOfWork unitOfWork)
        {
            var xmlUnicoTssString = unitOfWork.TransmissaoNFSBL.SerializeNotaNFS(entity);
            var xmlBase64 = Base64Helper.CodificaBase64(xmlUnicoTssString);

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

            var response = new TransmissaoNFSRetornoVM()
            {
                NotaId = entity.ItemTransmissaoNFSVM.NotaId,
                XMLUnicoTSS = xmlUnicoTssString
            };

            ///Essa prefeitura não tem schema, o schema é validado pela prefeitura..
            if (entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE != "4104204")
            {
                var validacao = new NFSE001.NFSE001().SCHEMAX(AppDefault.Token, entity.Homologacao, entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE, notaSchema, false, false);
                response.XMLGerado = Encoding.UTF8.GetString(validacao[0]?.XML);

                if (!string.IsNullOrEmpty(validacao[0].MENSAGEM) && validacao[0].MENSAGEM.Trim().Length > 0)
                {

                    var schema = new SchemaXMLNFSRetornoVM
                    {
                        Id = validacao[0].ID,
                        Mensagem = validacao[0].MENSAGEM.Trim().Replace("\\", "").Replace("\n", "")
                    };
                    response.Error = schema;
                    return response;
                }
            }

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

            return response;
        }
    }
}