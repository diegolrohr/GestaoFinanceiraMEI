using System.Web.Http;
using Fly01.Compras.BL;
using System;
using Fly01.Core.API;
using Fly01.Core.Notifications;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("calculatotalordemCompra")]
    public class CalculaTotalOrdemCompraController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid ordemCompraId, string fornecedorId, bool geraNotaFiscal, string tipoVenda, string tipoFrete, double? valorFrete = 0, bool onList = false)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    return Ok(unitOfWork.PedidoBL.CalculaTotalOrdemCompra(ordemCompraId, fornecedorId.ToString(), geraNotaFiscal, tipoVenda, tipoFrete, valorFrete == null ? 0 : valorFrete, onList));
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}