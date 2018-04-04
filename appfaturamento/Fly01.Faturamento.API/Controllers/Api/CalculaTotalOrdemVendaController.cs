using System.Web.Http;
using Fly01.Core.Controllers.API;
using Fly01.Faturamento.BL;
using System;
using Fly01.Core.ValueObjects;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("calculatotalordemVenda")]
    public class CalculaTotalOrdemVendaController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid ordemVendaId, bool geraNotaFiscal, double? valorFreteCIF = 0, bool onList = false)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    return Ok(unitOfWork.OrdemVendaBL.CalculaTotalOrdemVenda(ordemVendaId, geraNotaFiscal, valorFreteCIF == null ? 0 : valorFreteCIF, onList));
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}