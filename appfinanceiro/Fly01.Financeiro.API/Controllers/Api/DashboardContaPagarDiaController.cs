using Fly01.Core.API;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.BL;
using System;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("dashboardcontapagardia")]
    public class DashboardContaPagarDiaController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime filtro, int pageNo = 1, int pageSize = 10)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                int skipRecords = (pageNo - 1) * pageSize;

                int totalRecords = unitOfWork.DashboardBL.GetContasPagarDoDiaCount(filtro);

                var items = unitOfWork.DashboardBL.GetDashContasPagarDoDia(filtro, skipRecords, pageSize);

                return Ok(new PagedResult<ContasPagarDoDiaVM>(items, pageNo, pageSize, totalRecords));
            }
        }

        [HttpGet]
        public IHttpActionResult Get(DateTime dataFinal, DateTime dataInicial)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.ContaPagarBL.GetSaldoStatus(dataFinal, dataInicial));
            }
        }
    }
}