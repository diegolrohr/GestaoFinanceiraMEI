using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Financeiro.Domain.Entities;
using System.Web.Http;
using System.Web.OData;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("contapagar")]
    public class ContaPagarController : ApiPlataformaController<ContaPagar, ContaPagarBL>
    {
        public ContaPagarController()
        {
            MustProduceMessageServiceBus = true;
        }

        [EnableQuery(PageSize = 1000, MaxTop = 1000, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());   
        }
    }
}