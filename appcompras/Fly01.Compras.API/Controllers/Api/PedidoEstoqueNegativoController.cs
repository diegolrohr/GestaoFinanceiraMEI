using System.Web.Http;
using Fly01.Compras.BL;
using System;
using Fly01.Core.API;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("pedidoestoquenegativo")]
    public class PedidoEstoqueNegativoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid pedidoId, string tipoCompra)
         {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.PedidoBL.VerificaEstoqueNegativo(pedidoId, tipoCompra));
            }
        }
    }
}