using Fly01.Core.API;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.BL;
using System;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("dashboardcontareceberdia")]
    public class DashboardContaReceberDiaController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataFinal, DateTime dataInicial)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.ContaReceberBL.GetSaldoStatus(dataFinal, dataInicial));
            }
        }
    }
}