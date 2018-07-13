﻿using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("monitorevento")]
    public class MonitorEventoController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(MonitorEventoVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.MonitorEventoBL.ValidaModel(entity);

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

        public MonitorEventoRetornoVM Producao(MonitorEventoVM entity, UnitOfWork unitOfWork)
        {
            var retorno = new MonitorEventoRetornoVM();

            var monitor = new NFESBRAProd.NFESBRA().NFERETORNAEVENTO(
                AppDefault.Token,
                entity.Producao,
                entity.IdEvento,
                entity.SefazChaveAcesso
            );

            if (monitor.Length > 0)
            {
                NFESBRAProd.NFERETORNAEVENTO nfe = monitor[0];
                    retorno.IdEvento = nfe.ID_EVENTO;
                    retorno.Status = unitOfWork.MonitorEventoBL.ValidaStatus(nfe.CSTATEVEN);
                    retorno.Motivo = nfe.XMOTIVO;
                    retorno.MotivoEvento = nfe.XMOTIVOEVEN;
                    retorno.Protocolo = nfe.PROT;
                    //var xml = string.IsNullOrEmpty(nfe.XML_RET) ? "" : nfe.XML_RET +
                    //        string.IsNullOrEmpty(nfe.XML_SIG) ? "" : nfe.XML_SIG;
                    retorno.XML = "";
            }

            return retorno;
        }

        public MonitorEventoRetornoVM Homologacao(MonitorEventoVM entity, UnitOfWork unitOfWork)
        {
            var retorno = new MonitorEventoRetornoVM();

            var monitor = new NFESBRA.NFESBRA().NFERETORNAEVENTO(
                AppDefault.Token,
                entity.Homologacao,
                entity.IdEvento,
                entity.SefazChaveAcesso
            );

            if (monitor.Length > 0)
            {
                NFESBRA.NFERETORNAEVENTO nfe = monitor[0];
                retorno.IdEvento = nfe.ID_EVENTO;
                retorno.Status = unitOfWork.MonitorEventoBL.ValidaStatus(nfe.CSTATEVEN);
                retorno.Motivo = nfe.XMOTIVO;
                retorno.MotivoEvento = nfe.XMOTIVOEVEN;
                retorno.Protocolo = nfe.PROT;
                //var xml = string.IsNullOrEmpty(nfe.XML_RET) ? "" : nfe.XML_RET +
                //        string.IsNullOrEmpty(nfe.XML_SIG) ? "" : nfe.XML_SIG;
                retorno.XML = "";
            }

            return retorno;
        }
    }
}