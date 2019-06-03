using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.API.NFSE001;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("monitorNFS")]
    public class MonitorNFSController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(MonitorNFSVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.MonitorNFSBL.ValidaModel(entity);

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

        public ListMonitorNFSRetornoVM Producao(MonitorNFSVM entity, UnitOfWork unitOfWork)
        {
            var result = new ListMonitorNFSRetornoVM();
            result.Retornos = new List<MonitorNFSRetornoVM>();

            var monitor = new NFSE001Prod.NFSE001().MONITORX(
                AppDefault.Token,
                entity.Producao,
                "1",
                entity.NotaInicial,
                entity.NotaFinal,
                entity.DataInicial,
                entity.DataFinal,
                "00:00:00",
                "00:00:00",
                "0",
                null,
                null,
                null
            );

            if (monitor.Length > 0)
            {
                foreach (NFSE001Prod.MONITORNFSE nfse in monitor)
                {
                    var retorno = new MonitorNFSRetornoVM();
                    StatusNFSTSS statusNFSTSS = (StatusNFSTSS)Enum.Parse(typeof(StatusNFSTSS), nfse.STATUS, true);

                    retorno.NotaFiscalId = nfse?.ID;
                    retorno.Modalidade = nfse?.MODALIDADE;
                    retorno.Recomendacao = nfse?.RECOMENDACAO;
                    retorno.Status = nfse?.STATUS;
                    retorno.Protocolo = nfse?.PROTOCOLO?.Trim();
                    //TODO: Diego
                    if (statusNFSTSS == StatusNFSTSS.Autorizada)
                    {
                        NFS5 nfsXMl = new NFSE001.NFSE001().RETORNANFSE(
                            AppDefault.Token,
                            entity.Producao,
                            new NFSID()
                            {
                                NOTAS = new NFSESID1[1]
                                {
                                    new NFSESID1()
                                    {
                                        ID = nfse.ID
                                    }
                                }
                            },
                            null
                        );
                        if(nfsXMl != null)
                        {
                            var xml = nfsXMl?.NOTAS[0]?.NFE?.XML;
                            retorno.XML = xml != null ? xml.Replace("<![CDATA[", "").Replace("]]>", "") : "";
                        }
                    }

                    if (nfse.ERRO.Length != 0)
                    {
                        retorno.Erros = new List<ErroNFSVM>();
                        foreach (NFSE001Prod.ERROSLOTE erro in nfse.ERRO)
                        {
                            retorno.Erros.Add(new ErroNFSVM()
                            {
                                Codigo = (!string.IsNullOrEmpty(erro.CODIGO) ? erro.CODIGO : ""),
                                Mensagem = (!string.IsNullOrEmpty(erro.MENSAGEM) ? erro.MENSAGEM : "")
                            });
                        }
                    }

                    result.Retornos.Add(retorno);
                }
            }

            return result;
        }

        public ListMonitorNFSRetornoVM Homologacao(MonitorNFSVM entity, UnitOfWork unitOfWork)
        {
            var result = new ListMonitorNFSRetornoVM();
            result.Retornos = new List<MonitorNFSRetornoVM>();

            var monitor = new NFSE001.NFSE001().MONITORX(
                AppDefault.Token,
                entity.Homologacao,
                "1",
                entity.NotaInicial,
                entity.NotaFinal,
                entity.DataInicial,
                entity.DataFinal,
                "00:00:00",
                "00:00:00",
                "0",
                null,
                null,
                null
            );

            if (monitor.Length > 0)
            {
                foreach (NFSE001.MONITORNFSE nfse in monitor)
                { 
                    var retorno = new MonitorNFSRetornoVM();
                    retorno.NotaFiscalId = nfse.ID;
                    retorno.Modalidade = nfse.MODALIDADE;
                    retorno.Recomendacao = nfse.RECOMENDACAO;
                    retorno.Status = nfse?.STATUS;
                    retorno.Protocolo = nfse.PROTOCOLO.Trim();
                    //TODO: Refatorar
                    retorno.XML = nfse.XMLRETTSS;
                    if (nfse.ERRO.Length != 0)
                    {
                        retorno.Erros = new List<ErroNFSVM>();
                        foreach (NFSE001.ERROSLOTE erro in nfse.ERRO)
                        {
                            retorno.Erros.Add(new ErroNFSVM()
                            {
                                Codigo = (!string.IsNullOrEmpty(erro.CODIGO) ? erro.CODIGO : ""),
                                Mensagem = (!string.IsNullOrEmpty(erro.MENSAGEM) ? erro.MENSAGEM : "")
                            });
                        }
                    }

                    result.Retornos.Add(retorno);
                }
            }
            return result;
        }
    }
}
