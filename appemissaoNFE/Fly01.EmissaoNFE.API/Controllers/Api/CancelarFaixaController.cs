using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("cancelarFaixa")]
    public class CancelarFaixaController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(CancelarFaixaVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.CancelarFaixaBL.ValidaModel(entity);

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

        public List<CancelarFaixaRetornoVM> Producao(CancelarFaixaVM entity)
        {
            var response = new List<CancelarFaixaRetornoVM>();
            var nota = new CancelarFaixaRetornoVM();

            var monitor = new NFESBRAProd.NFESBRA().CANCELAFAIXA(
                AppDefault.Token,
                entity.Producao,
                entity.NotaInicial,
                entity.NotaFinal,
                ""
            );

            if (monitor.ID.Length > 0)
            {
                for (int x = 0; x < monitor.ID.Length; x++)
                {
                    nota.Notas = monitor.ID[x];

                    response.Add(nota);
                }
            }

            return response;
        }

        public List<CancelarFaixaRetornoVM> Homologacao(CancelarFaixaVM entity)
        {
            var response = new List<CancelarFaixaRetornoVM>();
            var nota = new CancelarFaixaRetornoVM();

            var monitor = new NFESBRA.NFESBRA().CANCELAFAIXA(
                AppDefault.Token,
                entity.Homologacao,
                entity.NotaInicial,
                entity.NotaFinal,
                ""
            );

            if (monitor.ID.Length > 0)
            {
                for (int x = 0; x < monitor.ID.Length; x++)
                {
                    nota.Notas = monitor.ID[x];

                    response.Add(nota);
                }
            }

            return response;
        }
    }
}
