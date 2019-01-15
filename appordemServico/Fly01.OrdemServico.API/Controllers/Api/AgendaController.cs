using Fly01.Core.API;
using Fly01.OrdemServico.BL;
using System;
using System.Web.Http;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [RoutePrefix("agenda")]
    public class AgendaController: ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataFinal, DateTime dataInicial)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.AgendaBL.GetCalendar(dataInicial, dataFinal));
            }
        }
    }
}