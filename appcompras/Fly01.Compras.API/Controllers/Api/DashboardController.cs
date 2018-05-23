using Fly01.Core.API;
using System.Web.Http;

namespace Fly01.Compras.API.Controllers.Api
{
    [AllowAnonymous]
    [RoutePrefix("dashboard")]
    public class DashboardController : ApiBaseController
    {
        //[HttpGet]
        //[Route("produtosmaiscomprados")]
        //public IHttpActionResult GetProdutosMaisComprados(DateTime filtro)
        //{
        //    using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
        //    {
        //        return Ok(unitOfWork.DashboardBL.getProdutosMaisComprados(filtro));
        //    }
        //}

        //[HttpGet]
        //[Route("dashboardstatus")]
        //public IHttpActionResult GetStatus(DateTime filtro, TipoOrdemCompra tipo)
        //{
        //    using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
        //    {
        //        return Ok(unitOfWork.DashboardBL.GetComprasStatus(filtro, tipo));
        //    }
        //}
    }
}