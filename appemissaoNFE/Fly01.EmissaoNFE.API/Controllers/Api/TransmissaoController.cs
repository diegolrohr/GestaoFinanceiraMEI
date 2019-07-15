using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Fly01.Core.Helpers;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("transmissao")]
    public class TransmissaoController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(TransmissaoVM entity)
        {
            entity?.TrimAllStrings();

            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.TransmissaoBL.ValidaModel(entity);

                unitOfWork.IbptNcmBL.CalculaImpostoIBPT(entity);

                unitOfWork.TransmissaoBL.MensagemCreditoICMS(entity);

                foreach (var item in entity.Item)
                {
                    item.Identificador.CodigoNF = unitOfWork.ChaveBL.CodificaCodigoNF(item.Identificador.CodigoNF, entity.EntidadeAmbiente);
                    item.NotaId = unitOfWork.ChaveBL.GeraChave(
                                    item.Identificador.CodigoUF.ToString(),
                                    item.Identificador.Emissao.Year.ToString(),
                                    item.Identificador.Emissao.Month.ToString(),
                                    item.Emitente.Cnpj,
                                    item.Identificador.ModeloDocumentoFiscal.ToString(),
                                    item.Identificador.Serie.ToString(),
                                    item.Identificador.NumeroDocumentoFiscal.ToString(),
                                    ((int)item.Identificador.FormaEmissao).ToString(),
                                    item.Identificador.CodigoNF.ToString()
                                );
                    unitOfWork.ResponsavelTecnicoBL.TagResponsavelTecnico(item, entity.EntidadeAmbiente);                  
                }

                try
                {
                    var retorno = (int)entity.EntidadeAmbiente == 2 ? Homologacao(entity, unitOfWork) : Producao(entity, unitOfWork);
                    //var retorno = new TransmissaoRetornoVM();
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

        public TransmissaoRetornoVM Producao(TransmissaoVM entity, UnitOfWork unitOfWork)
        {
            var listNotasValidas = new List<NFESBRAProd.NFES>();
            var notasValidas = new NFESBRAProd.NFE();

            var retorno = new TransmissaoRetornoVM()
            {
                Notas = new List<ItemTransmissaoRetornoVM>()
            };
            foreach (var nota in entity.Item)
            {
                var xmlString = unitOfWork.TransmissaoBL.SerializeNota(nota);
                var base64 = Base64Helper.CodificaBase64(xmlString);

                #region Validação do base64 via API Schema do TSS
                var notaSchema = new NFESBRAProd.NFE();

                var listNota = new List<NFESBRAProd.NFES>();
                listNota.Add(new NFESBRAProd.NFES
                {
                    ID = nota.NotaId.Replace("NFe", ""),
                    MAIL = "",
                    XML = Convert.FromBase64String(base64)
                });

                notaSchema.NOTAS = listNota.ToArray();

                var validacao = new NFESBRAProd.NFESBRA().SCHEMA(
                    AppDefault.Token,
                    entity.Producao,
                    notaSchema
                );

                var mensagens = new SchemaXMLMensagemVM();
                var response = new ItemTransmissaoRetornoVM();
                response.NotaId = nota.NotaId.Replace("NFe", "");
                response.XML = xmlString;

                if (validacao[0].SCHEMAMSG.Length > 0)
                {
                    response.Error = new List<SchemaXMLRetornoVM>();
                    var schema = new SchemaXMLRetornoVM();
                    schema.NotaId = validacao[0].ID;
                    schema.Mensagem = validacao[0].MENSAGEM.Replace("\\", "").Replace("\n", "");
                    schema.XML = Base64Helper.CodificaBase64(Convert.ToBase64String(validacao[0].XML));

                    if (validacao[0].SCHEMAMSG.Length > 0)
                    {
                        schema.SchemaMensagem = new List<SchemaXMLMensagemVM>();
                        foreach (var erro in validacao[0].SCHEMAMSG)
                        {
                            mensagens.Descricao = erro.DESC ?? "";
                            mensagens.Erro = erro.ERRO ?? "";
                            mensagens.Resumo = erro.LOG ?? "";
                            mensagens.Local = erro.PARENT ?? "";
                            mensagens.Campo = erro.TAG ?? "";

                            schema.SchemaMensagem.Add(mensagens);
                        }
                    }

                    response.Error.Add(schema);
                }

                #endregion

                #region Montagem da lista de notas válidas
                else
                {
                    listNotasValidas.Add(new NFESBRAProd.NFES
                    {
                        ID = nota.NotaId.Replace("NFe", ""),
                        MAIL = "",
                        XML = Convert.FromBase64String(base64)
                    });
                }
                #endregion

                retorno.Notas.Add(response);
            }

            if (!retorno.Notas.Any(y => y.Error != null))
            {
                #region Transmissão da nota

                notasValidas.NOTAS = listNotasValidas.ToArray();

                var x = new NFESBRAProd.NFESBRA().REMESSA(AppDefault.Token, entity.Producao, notasValidas);

                #endregion
            }

            return retorno;
        }

        public TransmissaoRetornoVM Homologacao(TransmissaoVM entity, UnitOfWork unitOfWork)
        {
            var listNotasValidas = new List<NFESBRA.NFES>();
            var notasValidas = new NFESBRA.NFE();

            var retorno = new TransmissaoRetornoVM();
            retorno.Notas = new List<ItemTransmissaoRetornoVM>();

            foreach (var nota in entity.Item)
            {
                var xmlString = unitOfWork.TransmissaoBL.SerializeNota(nota);
                var base64 = Base64Helper.CodificaBase64(xmlString);

                #region Validação do base64 via API Schema do TSS
                var notaSchema = new NFESBRA.NFE();

                var listNota = new List<NFESBRA.NFES>();
                listNota.Add(new NFESBRA.NFES
                {
                    ID = nota.NotaId.Replace("NFe", ""),
                    MAIL = "",
                    XML = Convert.FromBase64String(base64)
                });

                notaSchema.NOTAS = listNota.ToArray();

                var validacao = new NFESBRA.NFESBRA().SCHEMA(
                    AppDefault.Token,
                    entity.Homologacao,
                    notaSchema
                );

                var mensagens = new SchemaXMLMensagemVM();
                var response = new ItemTransmissaoRetornoVM();
                response.NotaId = nota.NotaId.Replace("NFe", "");
                response.XML = xmlString;

                if (validacao[0].SCHEMAMSG.Length > 0)
                {
                    response.Error = new List<SchemaXMLRetornoVM>();
                    var schema = new SchemaXMLRetornoVM();
                    schema.NotaId = validacao[0].ID;
                    schema.Mensagem = validacao[0].MENSAGEM.Replace("\\", "").Replace("\n", "");
                    schema.XML = Base64Helper.CodificaBase64(Convert.ToBase64String(validacao[0].XML));

                    if (validacao[0].SCHEMAMSG.Length > 0)
                    {
                        schema.SchemaMensagem = new List<SchemaXMLMensagemVM>();
                        foreach (var erro in validacao[0].SCHEMAMSG)
                        {
                            mensagens.Descricao = erro.DESC ?? "";
                            mensagens.Erro = erro.ERRO ?? "";
                            mensagens.Resumo = erro.LOG ?? "";
                            mensagens.Local = erro.PARENT ?? "";
                            mensagens.Campo = erro.TAG ?? "";

                            schema.SchemaMensagem.Add(mensagens);
                        }
                    }

                    response.Error.Add(schema);
                }

                #endregion

                #region Montagem da lista de notas válidas
                else
                {
                    listNotasValidas.Add(new NFESBRA.NFES
                    {
                        ID = nota.NotaId.Replace("NFe", ""),
                        MAIL = "",
                        XML = Convert.FromBase64String(base64)
                    });
                }
                #endregion

                retorno.Notas.Add(response);
            }

            if (!retorno.Notas.Any(y => y.Error != null))
            {
                #region Transmissão da nota

                notasValidas.NOTAS = listNotasValidas.ToArray();

                var x = new NFESBRA.NFESBRA().REMESSA(AppDefault.Token, entity.Homologacao, notasValidas);

                #endregion
            }

            return retorno;
        }
    }
}