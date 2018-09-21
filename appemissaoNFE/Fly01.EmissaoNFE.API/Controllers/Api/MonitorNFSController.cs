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

        public ListMonitorNFSRetornoVM Producao(MonitorNFSVM entity, UnitOfWork unitOfWork)
        {
            var retorno = new ListMonitorNFSRetornoVM();
            retorno.Retornos = new List<MonitorNFSRetornoVM>();
            //REMESSANFSE001
            //    só codigo municipio
            //    id = serie+numeronfs
            //xml e demais null
            // REPROC = 1 so o de fora

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
                    //1 2 3                     
                    //exibir a lista de erros
                    var nota = new MonitorNFSRetornoVM();
                    nota.NotaFiscalId = nfse.ID;
                    nota.Status = unitOfWork.MonitorNFSBL.ValidaStatus(nfse.PROTOCOLO, entity.StatusNotaFiscalAnterior);
                    nota.Modalidade = nfse.MODALIDADE;
                    nota.Recomendacao = nfse.RECOMENDACAO;
                    nota.Protocolo = nfse.PROTOCOLO;
                    nota.XML = nfse.XMLRETTSS;
                    //se tem ERRO new ErrosNFSVM 
                    //foreach add de new ErroNFSVM {codigo = erro[i].codigo

                    retorno.Retornos.Add(nota);
                }
            }

            return retorno;
        }

        public ListMonitorNFSRetornoVM Homologacao(MonitorNFSVM entity, UnitOfWork unitOfWork)
        {
            return new ListMonitorNFSRetornoVM
            {
                Retornos = new List<MonitorNFSRetornoVM>()
            };
        }
    }
}
