using Fly01.Core.API;
using Fly01.OrdemServico.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [RoutePrefix("dashboardtopservicos")]
    public class DashboardTopServicosController : ApiBaseController
    {
        public IHttpActionResult Get(DateTime filtro)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.DashboardBL.GetTopServicosOrdemServico(filtro));
            }
        }
    }
}
