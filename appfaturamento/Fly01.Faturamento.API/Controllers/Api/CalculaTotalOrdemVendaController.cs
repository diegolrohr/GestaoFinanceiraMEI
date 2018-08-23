using System.Web.Http;
using Fly01.Faturamento.BL;
using System;
using Fly01.Core.API;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("calculatotalordemVenda")]
    public class CalculaTotalOrdemVendaController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid ordemVendaId, Guid clienteId, bool geraNotaFiscal, string tipoNfeComplementar, string tipoFrete, double? valorFrete = 0, bool onList = false)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    return Ok(unitOfWork.OrdemVendaBL.CalculaTotalOrdemVenda(ordemVendaId, clienteId, geraNotaFiscal, tipoNfeComplementar, tipoFrete, valorFrete == null ? 0 : valorFrete, onList));
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}