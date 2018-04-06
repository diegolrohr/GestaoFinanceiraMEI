using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("schemaXML")]
    public class SchemaXMLController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(SchemaXMLVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                //unitOfWork.SchemaXMLBL.ValidaModel(entity);
                
                try
                {
                    var response = (int)entity.EntidadeAmbiente == 2 ? Homologacao(entity) : Producao(entity);
                    
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

        public ListSchemaXMLRetornoVM Producao(SchemaXMLVM entity)
        {
            var notas = new NFESBRAProd.NFE();
            var listNotas = new List<NFESBRAProd.NFES>();

            foreach (var nota in entity.Notas)
            {
                listNotas.Add(new NFESBRAProd.NFES
                {
                    ID = nota.NotaId,
                    MAIL = nota.Email,
                    XML = Convert.FromBase64String(nota.XML)
                });

                notas.NOTAS = listNotas.ToArray();
            }

            var validacao = new NFESBRAProd.NFESBRA().SCHEMA(
                AppDefault.Token,
                entity.Producao,
                notas
            );

            var mensagens = new SchemaXMLMensagemVM();
            var response = new ListSchemaXMLRetornoVM();
            response.Retorno = new List<SchemaXMLRetornoVM>();

            foreach (var item in validacao)
            {
                var schema = new SchemaXMLRetornoVM();
                schema.NotaId = item.ID;
                schema.Mensagem = item.MENSAGEM.Replace("\\", "").Replace("\n", "");
                schema.XML = Convert.ToBase64String(item.XML);

                if (item.SCHEMAMSG.Length > 0)
                {
                    schema.SchemaMensagem = new List<SchemaXMLMensagemVM>();
                    foreach (var erro in item.SCHEMAMSG)
                    {
                        mensagens.Descricao = erro.DESC;
                        mensagens.Erro = erro.ERRO;
                        mensagens.Resumo = erro.LOG;
                        mensagens.Local = erro.PARENT;
                        mensagens.Campo = erro.TAG;

                        schema.SchemaMensagem.Add(mensagens);
                    }
                }

                response.Retorno.Add(schema);
            }

            return response;
        }

        public ListSchemaXMLRetornoVM Homologacao(SchemaXMLVM entity)
        {
            var notas = new NFESBRA.NFE();
            var listNotas = new List<NFESBRA.NFES>();

            foreach (var nota in entity.Notas)
            {
                listNotas.Add(new NFESBRA.NFES
                {
                    ID = nota.NotaId,
                    MAIL = nota.Email,
                    XML = Convert.FromBase64String(nota.XML)
                });

                notas.NOTAS = listNotas.ToArray();
            }

            var validacao = new NFESBRA.NFESBRA().SCHEMA(
                AppDefault.Token,
                entity.Homologacao,
                notas
            );

            var mensagens = new SchemaXMLMensagemVM();
            var response = new ListSchemaXMLRetornoVM();
            response.Retorno = new List<SchemaXMLRetornoVM>();

            foreach (var item in validacao)
            {
                var schema = new SchemaXMLRetornoVM();
                schema.NotaId = item.ID;
                schema.Mensagem = item.MENSAGEM.Replace("\\", "").Replace("\n", "");
                schema.XML = Convert.ToBase64String(item.XML);

                if (item.SCHEMAMSG.Length > 0)
                {
                    schema.SchemaMensagem = new List<SchemaXMLMensagemVM>();
                    foreach (var erro in item.SCHEMAMSG)
                    {
                        mensagens.Descricao = erro.DESC;
                        mensagens.Erro = erro.ERRO;
                        mensagens.Resumo = erro.LOG;
                        mensagens.Local = erro.PARENT;
                        mensagens.Campo = erro.TAG;

                        schema.SchemaMensagem.Add(mensagens);
                    }
                }

                response.Retorno.Add(schema);
            }

            return response;
        }
    }
}
