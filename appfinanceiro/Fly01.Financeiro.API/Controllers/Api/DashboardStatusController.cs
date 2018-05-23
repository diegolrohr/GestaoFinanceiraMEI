using Fly01.Core.API;
using Fly01.Financeiro.BL;
using System;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("dashboardstatus")]
    public class DashboardStatusController : ApiBaseController
    {
        public IHttpActionResult Get(DateTime filtro, string tipo)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetDashFinanceiroPorStatus(filtro, tipo));
            }
        }
    }
}