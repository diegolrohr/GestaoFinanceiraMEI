using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using System.Web.Http;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("arquivo")]
    public class ArquivoController : ApiPlataformaController<Arquivo, ArquivoBL>
    {
        public ArquivoController()
        {
            MustProduceMessageServiceBus = true;
        }

        public override async Task<IHttpActionResult> Post(Arquivo entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            ModelState.Clear();

            Insert(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            await UnitSave();

            if (MustProduceMessageServiceBus)
                AfterSave(entity);

            return Created(entity);
        }
    }
}