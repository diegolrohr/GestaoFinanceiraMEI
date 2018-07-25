using Fly01.Compras.BL;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiBaseController
    {
        [HttpGet]
        [Route("formapagamento")]
        public IHttpActionResult GetFormaPagamento(DateTime filtro, string tipo)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetComprasFormasPagamento(filtro, tipo));
            }
        }

        [HttpGet]
        [Route("categoria")]
        public IHttpActionResult GetCategoria(DateTime filtro, string tipo)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetComprasCategoria(filtro, tipo));
            }
        }

        [HttpGet]
        [Route("status")]
        public IHttpActionResult GetStatus(DateTime filtro, string tipo)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetComprasStatus(filtro, tipo));
            }
        }

        [HttpGet]
        [Route("maioresfornecedores")]
        public IHttpActionResult GetMaioresFornecedores(DateTime filtro)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetMaioresFornecedores(filtro));
            }
        }

        [HttpGet]
        [Route("produtosmaiscomprados")]
        public IHttpActionResult GetProdutosMaisComprados(DateTime filtro)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetProdutosMaisComprados(filtro));
            }
        }
    }
}