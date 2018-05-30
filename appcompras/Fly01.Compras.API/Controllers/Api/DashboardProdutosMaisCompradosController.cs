using Fly01.Compras.BL;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("dashboardprodutosmaiscomprados")]
    public class DashboardProdutosMaisCompradosController : ApiBaseController
    {
        public IHttpActionResult Get(DateTime filtro)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.getProdutosMaisComprados(filtro));
            }
        }
    }
}