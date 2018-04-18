using Fly01.Financeiro.BL;
using System.Web.Http;
using System.Linq;
using Fly01.Core.API;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("contapagarmaxrecords")]
    public class ContaPagarMaxRecordsController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {                
                return Ok(unitOfWork.ContaPagarBL.All.Take(1000).ToList());
            }
        }
    }
}