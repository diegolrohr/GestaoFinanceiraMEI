using Fly01.Core.API;
using Fly01.Financeiro.BL;
using System;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("dashboardcategoria")]
    public class DashboardCategoriaController : ApiBaseController
    {
        public IHttpActionResult Get(DateTime filtro, string tipo)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetDashFinanceiroCategoria(filtro, tipo));
            }
        }
    }
}