using Fly01.Financeiro.BL.Base;
using Fly01.Financeiro.Domain.Base;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Fly01.Financeiro.API.Controllers.Base
{
    public class ApiPlataformaBaseController<TEntity, TBL> : ApiDomainBaseController<TEntity, TBL>
        where TEntity : PlataformaBase, new()
        where TBL : PlataformaBaseBL<TEntity>
    {
        public ApiPlataformaBaseController()
        {
        }

        [EnableQuery(PageSize = 50, MaxTop = 50, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(unitOfWork.GetGenericBL<TBL>().All.AsQueryable());
        }

        [EnableQuery]
        [ODataRoute("({key})")]
        public override IHttpActionResult Get([FromODataUri] Guid key)
        {
            return Ok(SingleResult.Create(unitOfWork.GetGenericBL<TBL>()
                .All.Where(x => x.Id == key).AsQueryable()));
        }

        public virtual async Task<IHttpActionResult> Post(TEntity entity)
        {
            if (entity == null)
            {
                return BadRequest(ModelState);
            }

            ModelState.Clear();

            unitOfWork.GetGenericBL<TBL>().Insert(entity);

            Validate(entity);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await unitOfWork.Save(PlataformaUrl, UserName);
            return Created(entity);
        }

        public virtual async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<TEntity> model)
        {
            if (model == null || key == default(Guid) || key == null)
            {
                return BadRequest(ModelState);
            }

            var entity = unitOfWork.GetGenericBL<TBL>().Find(key);
            if (entity == null || !entity.Ativo)
            {
                return NotFound();
            }

            ModelState.Clear();
            model.Patch(entity);
            unitOfWork.GetGenericBL<TBL>().Update(entity);

            Validate(entity);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {                
                await unitOfWork.Save(PlataformaUrl, UserName);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!unitOfWork.GetGenericBL<TBL>().Exists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        public virtual async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            if (key == default(Guid) || key == null)
            {
                return BadRequest();
            }

            TEntity entity = unitOfWork.GetGenericBL<TBL>().Find(key);
            if (entity == null || !entity.Ativo)
            {
                return NotFound();
            }

            unitOfWork.GetGenericBL<TBL>().Delete(entity);

            await unitOfWork.Save(PlataformaUrl, UserName);
            return StatusCode(HttpStatusCode.NoContent);
        }

        //public virtual async Task<IHttpActionResult> DeleteSave([FromODataUri] Guid key, bool saveChanges = true)
        //{
        //    if (key == default(Guid) || key == null)
        //    {
        //        return BadRequest();
        //    }

        //    TEntity entity = unitOfWork.GetGenericBL<TBL>().Find(key);
        //    if (entity == null || !entity.Ativo)
        //    {
        //        return NotFound();
        //    }
        //    entity.Ativo = false;
        //    entity.DataExclusao = DateTime.Now;
        //    entity.UsuarioExclusao = UserName;

        //    if (!saveChanges)
        //        return StatusCode(HttpStatusCode.NoContent);

        //    return await base.Delete();
        //}
    }
}
