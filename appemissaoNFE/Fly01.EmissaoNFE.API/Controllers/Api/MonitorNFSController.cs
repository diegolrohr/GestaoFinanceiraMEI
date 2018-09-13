using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("monitornfs")]
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

        public ListMonitorRetornoVM Producao(MonitorNFSVM entity, UnitOfWork unitOfWork)
        {
            var retorno = new ListMonitorRetornoVM();
            retorno.Retornos = new List<MonitorRetornoVM>();

            //var monitor = new NFSE001Prod.NFSE001().MONITORX(
            //    AppDefault.Token,
            //    entity.Producao,
            //    entity.NotaInicial,
            //    entity.NotaFinal,
            //    ""
            //);

            //if (monitor.Length > 0)
            //{
            //    foreach (NFSE001Prod.MONITORNFSE nfse in monitor)
            //    {
            //        var nota = new MonitorRetornoVM();
            //        nota.NotaId = nfse.ID;
            //        nota.Status = unitOfWork.MonitorNFSBL.ValidaStatus(nfse.ERRO[nfse.ERRO.Length - 1].CODRETnfse, nfse.RECOMENDACAO);
            //        nota.Modalidade = nfse.MODALIDADE;
            //        nota.Recomendacao = nfse.RECOMENDACAO;
            //        nota.Mensagem = nfse.ERRO[nfse.ERRO.Length - 1].MSGRETnfse;
            //        nota.Data = nfse.ERRO[nfse.ERRO.Length - 1].DATALOTE;
            //        nota.Hora = nfse.ERRO[nfse.ERRO.Length - 1].HORALOTE;

            //        retorno.Retornos.Add(nota);
            //    }
            //}

            return retorno;
        }

        public ListMonitorRetornoVM Homologacao(MonitorNFSVM entity, UnitOfWork unitOfWork)
        {
            return new ListMonitorRetornoVM
            {
                Retornos = new List<MonitorRetornoVM>()
            };
        }
    }
}
