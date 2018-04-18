using System.Web.Http;
using Fly01.Faturamento.BL;
using System;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

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