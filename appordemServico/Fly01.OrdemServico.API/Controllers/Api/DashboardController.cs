using Fly01.Core.API;
using Fly01.OrdemServico.BL;
using System;
using System.Web.Http;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiBaseController
    {
        [HttpGet]
        [Route("status")]
        public IHttpActionResult GetOrdemServicoPorStatus(DateTime dataFinal, DateTime dataInicial)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetOrdemServicoPorStatus(dataFinal, dataInicial));
            }
        }

        [HttpGet]
        [Route("quantidadeordemservicopordia")]
        public IHttpActionResult GetQuantidadeOrdemServicoPorDia(DateTime filtro)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(new { value = unitOfWork.DashboardBL.GetQuantidadeOrdemServicoPorDia(filtro)});
            }
        }

        [HttpGet]
        [Route("topprodutosordemservico")]
        public IHttpActionResult GetTopProdutosOrdemServico(DateTime filtro)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetTopProdutosOrdemServico(filtro));
            }
        }

        [HttpGet]
        [Route("topservicosordemservico")]
        public IHttpActionResult GetTopServicosOrdemServico(DateTime filtro)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetTopServicosOrdemServico(filtro));
            }
        }
    }
}
