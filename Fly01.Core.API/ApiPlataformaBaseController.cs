using System;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using Fly01.Core.Entities.Domains;
using System.Threading.Tasks;
using Fly01.Core.Notifications;
using System.Web.OData.Routing;
using System.Web.Http.ModelBinding;
using Fly01.Core.ServiceBus;
using System.Data.Entity.Infrastructure;

namespace Fly01.Core.API
{
    [CustomExceptionFilter]
    public abstract class ApiPlataformaBaseController<TEntity> : ApiDomainBaseController<TEntity>
        where TEntity : PlataformaBase, new()
    {
        protected abstract void Insert(TEntity entity);
        protected abstract void Delete(TEntity primaryKey);
        protected abstract TEntity Find(object id);
        public bool MustProduceMessageServiceBus { get; set; }

        [EnableQuery(PageSize = 50, MaxTop = 50, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }

        [EnableQuery]
        [ODataRoute("({key})")]
        public override IHttpActionResult Get([FromODataUri] Guid key)
        {
            if (!All().Any(x => x.Id == key))
            {
                throw new BusinessException("Registro não encontrado ou já excluído");
            }
            else
            {
                return Ok(SingleResult.Create(All().Where(x => x.Id == key).AsQueryable()));
            }
        }

        public virtual async Task<IHttpActionResult> Post(TEntity entity)
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
                Producer.Send(entity.GetType().Name, entity, RabbitConfig.enHTTPVerb.POST);

            return Created(entity);
        }

        public virtual async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<TEntity> model)
        {
            if (model == null || key == default(Guid) || key == null)
                return BadRequest(ModelState);

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                throw new BusinessException("Registro não encontrado ou já excluído");

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
                    Producer.Send(entity.GetType().Name, entity, RabbitConfig.enHTTPVerb.PUT);
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

        public virtual async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            if (key == default(Guid) || key == null)
                return BadRequest();

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                throw new BusinessException("Registro não encontrado ou já excluído");

            Delete(entity);

            await UnitSave();

            if (MustProduceMessageServiceBus)
                Producer.Send(entity.GetType().Name, entity, RabbitConfig.enHTTPVerb.DELETE);

            return StatusCode(HttpStatusCode.NoContent);
        }

        private Notification Notification { get; } = new Notification();

        private void AddErrorModelState(ModelStateDictionary modelState)
        {
            modelState.ToList().ForEach(
                model => model.Value.Errors.ToList().ForEach(
                    itemErro => Notification.Errors.Add(
                        new Error(itemErro.ErrorMessage, string.Concat(char.ToLowerInvariant(model.Key[0]), model.Key.Substring(1))))));

            throw new BusinessException(Notification.Get());
        }
    }
}