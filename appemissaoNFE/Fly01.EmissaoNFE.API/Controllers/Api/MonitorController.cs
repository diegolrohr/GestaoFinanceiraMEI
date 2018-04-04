using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Controllers.API;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("monitor")]
    public class MonitorController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(MonitorVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.MonitorBL.ValidaModel(entity);

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

        public ListMonitorRetornoVM Producao(MonitorVM entity, UnitOfWork unitOfWork)
        {
            var retorno = new ListMonitorRetornoVM();
            retorno.Retornos = new List<MonitorRetornoVM>();
            var nota = new MonitorRetornoVM();

            var monitor = new NFESBRAProd.NFESBRA().MONITORFAIXA(
                AppDefault.Token,
                entity.Producao,
                entity.NotaInicial,
                entity.NotaFinal,
                ""
            );

            if (monitor.Length > 0)
            {
                foreach (NFESBRAProd.MONITORNFE nfe in monitor)
                {
                    nota.NotaId = nfe.ID;
                    nota.Status = unitOfWork.MonitorBL.ValidaStatus(nfe.ERRO[nfe.ERRO.Length - 1].CODRETNFE, nfe.RECOMENDACAO);
                    nota.Modalidade = nfe.MODALIDADE;
                    nota.Recomendacao = nfe.RECOMENDACAO;
                    nota.Mensagem = nfe.ERRO[nfe.ERRO.Length - 1].MSGRETNFE;
                    nota.Data = nfe.ERRO[nfe.ERRO.Length - 1].DATALOTE;
                    nota.Hora = nfe.ERRO[nfe.ERRO.Length - 1].HORALOTE;

                    retorno.Retornos.Add(nota);
                }
            }

            return retorno;
        }

        public ListMonitorRetornoVM Homologacao(MonitorVM entity, UnitOfWork unitOfWork)
        {
            var retorno = new ListMonitorRetornoVM();
            retorno.Retornos = new List<MonitorRetornoVM>();
            var nota = new MonitorRetornoVM();

            var monitor = new NFESBRA.NFESBRA().MONITORFAIXA(
                AppDefault.Token,
                entity.Homologacao,
                entity.NotaInicial,
                entity.NotaFinal,
                ""
            );

            if (monitor.Length > 0)
            {
                foreach (NFESBRA.MONITORNFE nfe in monitor)
                {
                    nota.NotaId = nfe.ID;
                    nota.Status = unitOfWork.MonitorBL.ValidaStatus(nfe.ERRO[nfe.ERRO.Length - 1].CODRETNFE, nfe.RECOMENDACAO);
                    nota.Modalidade = nfe.MODALIDADE;
                    nota.Recomendacao = nfe.RECOMENDACAO;
                    nota.Mensagem = nfe.ERRO[nfe.ERRO.Length - 1].MSGRETNFE;
                    nota.Data = nfe.ERRO[nfe.ERRO.Length - 1].DATALOTE;
                    nota.Hora = nfe.ERRO[nfe.ERRO.Length - 1].HORALOTE;

                    retorno.Retornos.Add(nota);
                }
            }

            return retorno;
        }
    }
}
