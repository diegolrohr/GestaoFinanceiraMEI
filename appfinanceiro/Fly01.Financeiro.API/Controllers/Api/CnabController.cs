using Fly01.Financeiro.BL;
using System;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Commons;
using System.Web.OData.Routing;
using System.Threading.Tasks;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("cnab")]
    public class CnabController : ApiPlataformaController<Cnab, CnabBL>
    {
        [HttpGet]
        public override IHttpActionResult Get(Guid Id)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.CnabBL.GetCnab(Id);
                return Ok(data);
            }
        }
    }
}