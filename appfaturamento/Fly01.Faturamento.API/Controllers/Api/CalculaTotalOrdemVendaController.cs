using System.Web.Http;
using Fly01.Faturamento.BL;
using System;
using Fly01.Core.API;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("calculatotalordemVenda")]
    public class CalculaTotalOrdemVendaController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid ordemVendaId, Guid clienteId, bool geraNotaFiscal, double? valorFreteCIF = 0, bool onList = false)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    return Ok(unitOfWork.OrdemVendaBL.CalculaTotalOrdemVenda(ordemVendaId, clienteId, geraNotaFiscal, valorFreteCIF == null ? 0 : valorFreteCIF, onList));
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}