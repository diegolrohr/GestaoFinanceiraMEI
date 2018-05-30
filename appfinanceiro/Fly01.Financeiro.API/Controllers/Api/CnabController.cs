using Fly01.Financeiro.BL;
using System;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Commons;
using System.Web.OData.Routing;
using System.Threading.Tasks;
using System.Linq;

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

        public async override Task<IHttpActionResult> Post(Cnab entity)
        {
            if (!base.All().Any(x => x.ContaReceberId == entity.ContaReceberId))
                return await base.Post(entity);

            return Ok();
        }
    }
}