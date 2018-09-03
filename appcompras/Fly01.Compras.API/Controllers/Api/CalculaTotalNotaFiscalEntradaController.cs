using System.Web.Http;
using Fly01.Compras.BL;
using System;
using Fly01.Core.API;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("calculatotalnotafiscalentrada")]
    public class CalculaTotalNotaFiscalEntradaController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid notaFiscalId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.NotaFiscalEntradaBL.CalculaTotalNotaFiscal(notaFiscalId));
            }
        }
    }
}