using System.Web.Http;
using Fly01.Core.Controllers.API;
using Fly01.Faturamento.BL;
using System;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("pedidoestoquenegativo")]
    public class PedidoEstoqueNegativoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid pedidoId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.OrdemVendaBL.VerificaEstoqueNegativo(pedidoId));
            }
        }
    }
}