using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using System.Web.OData;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("ordemvenda")]
    public class OrdemVendaController : ApiPlataformaController<OrdemVenda, OrdemVendaBL>
    {
        public OrdemVendaController()
        {
            MustProduceMessageServiceBus = true;
        }

        public override async Task<IHttpActionResult> Post(OrdemVenda entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            ModelState.Clear();

            Validate(entity);

            Insert(entity);

            await UnitSave();

            return Created(entity);
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<OrdemVenda> model)
        {
            if (model == null || key == default(Guid) || key == null)
                return BadRequest(ModelState);

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                return NotFound();

            ModelState.Clear();
            model.Patch(entity);
            Update(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            try
            {
                await UnitSave();

                if (MustProduceMessageServiceBus)
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