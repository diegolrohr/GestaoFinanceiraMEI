using System.Web.Http;
using Fly01.Core.Controllers.API;
using Fly01.Faturamento.BL;
using System;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("calculatotalnotafiscal")]
    public class CalculaTotalNotaFiscalController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid notaFiscalId, double? valorFreteCIF = 0)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.NotaFiscalBL.CalculaTotalNotaFiscal(notaFiscalId, valorFreteCIF));
            }
        }
    }
}