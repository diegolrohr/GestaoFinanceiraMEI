using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Enum;

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
                    var retorno = entity.EntidadeAmbiente == TipoAmbiente.Homologacao ? Homologacao(entity, unitOfWork) : Producao(entity, unitOfWork);

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
            //REMESSANFSE001
            //    só codigo municipio
            //    id = serie+numeronfs
            //xml e demais null
            // REPROC = 1 so o de fora

            //MONITORX
            //tipomonitor 1 fixo
            //ids serie+numero
            //dd/mm/yyyy
            //00:00:00
            //tempo 0 demais null

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
                    var nota = new MonitorRetornoVM();
                    nota.NotaId = nfse.ID;
                    //nota.Status = unitOfWork.MonitorNFSBL.ValidaStatus(nfse.ERRO[nfse.ERRO.Length - 1].CODIGO, nfse.RECOMENDACAO);
                    nota.Status = unitOfWork.MonitorNFSBL.ValidaStatus(nfse.STATUS, nfse.RECOMENDACAO);
                    nota.Modalidade = nfse.MODALIDADE;
                    nota.Recomendacao = nfse.RECOMENDACAO;
                    nota.Mensagem = nfse.ERRO[nfse.ERRO.Length - 1].MENSAGEM;

                    retorno.Retornos.Add(nota);
                }
            }

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
