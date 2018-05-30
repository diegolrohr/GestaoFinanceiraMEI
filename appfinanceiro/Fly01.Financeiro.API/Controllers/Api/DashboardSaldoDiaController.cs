using Fly01.Core.API;
using Fly01.Financeiro.BL;
using System;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("dashboardsaldodia")]
    public class DashboardSaldoDiaController : ApiBaseController
    {
        public IHttpActionResult Get(DateTime filtro)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetDashContasReceberPagoPorDia(filtro));
            }
        }
    }
}