using System;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("verificaRenegociacaoPedidoCompraVendaController")]
    public class VerificaRenegociacaoPedidoCompraVendaController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(RollbackFinanceiroCompraVenda entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok( new { value = unitOfWork.RollbackFinanceiroCompraVendaBL.VerificaContasRenegociadas(entity) } );
            }
        }
    }
}
