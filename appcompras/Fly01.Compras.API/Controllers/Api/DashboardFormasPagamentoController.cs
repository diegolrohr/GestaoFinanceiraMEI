using Fly01.Compras.BL;
using Fly01.Core.API;
using System;
using System.Web.Http;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("dashboardformaspagamento")]
    public class DashboardFormasPagamentoController : ApiBaseController
    {
        public IHttpActionResult Get(DateTime filtro, string tipo)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.getComprasFormasPagamento(filtro, tipo));
            }
        }
    }
}