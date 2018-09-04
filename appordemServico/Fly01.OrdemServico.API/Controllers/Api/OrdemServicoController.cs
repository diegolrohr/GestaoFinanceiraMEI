using Fly01.Core.Entities.Domains.Enum;
using Fly01.OrdemServico.BL;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [ODataRoutePrefix("ordemservico")]
    public class OrdemServicoController : ApiPlataformaController<Core.Entities.Domains.Commons.OrdemServico, OrdemServicoBL>
    {
        public OrdemServicoController()
        {
            MustProduceMessageServiceBus = false;
        }

        public override async Task<IHttpActionResult> Post(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            ModelState.Clear();

            Insert(entity);

            Validate(entity);

            await UnitSave();

            return Created(entity);
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Core.Entities.Domains.Commons.OrdemServico> model)
        {
            if (model == null || key == default(Guid) || key == null)
                return BadRequest(ModelState);

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                return NotFound();

            var lastState = entity.Status;
            var lastGera = entity.GeraOrdemVenda;
            var changedProperties = model.GetChangedPropertyNames().ToList();
            ModelState.Clear();
            model.Patch(entity);

            if (changedProperties.Count == 1 && changedProperties[0] == "GeraOrdemVenda")
                UnitOfWork.GetGenericBL<OrdemServicoBL>().GerarOrdemVenda(entity);
            else
                Update(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            try
            {
                await UnitSave();

                if ((lastState != entity.Status && entity.Status == StatusOrdemServico.Concluido) ||
                        (!lastGera && entity.GeraOrdemVenda))
                    AfterSave(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(key))
                    return NotFound();
                else
                    throw;
            }

            return Ok();
        }

        public override async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            if (key == default(Guid) || key == null)
                return BadRequest();

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                return NotFound();

            Delete(entity);

            await UnitSave();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}