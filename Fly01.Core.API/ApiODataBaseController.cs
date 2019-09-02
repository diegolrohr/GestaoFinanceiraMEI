using System;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using Fly01.Core.Base;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fly01.Core.Entities.Domains;
using System.Data.Entity.Infrastructure;

namespace Fly01.Core.API
{
    [CustomExceptionFilter]
    public abstract class ApiODataBaseController<TEntity> : ODataController where TEntity : DomainBase, new()
    {
        protected abstract IQueryable<TEntity> All(); //UnitOfWork.GetGenericBL<TBL>().All
        protected abstract void Update(TEntity entity); //UnitOfWork.GetGenericBL<TBL>().Update(entity)
        protected abstract bool Exists(object primaryKey); //UnitOfWork.GetGenericBL<TBL>().Exists(key)
        protected abstract void UnitDispose(bool disposing); //UnitOfWork.Dispose()
        protected abstract Task UnitSave(); //UnitOfWork.Save()

        private ContextInitialize _contextInitialize;
        protected ContextInitialize ContextInitialize
        {
            get
            {
                return _contextInitialize ?? (_contextInitialize = new ContextInitialize
                {
                    EmpresaId = EmpresaId,
                    AppUser = AppUser
                });
            }
            set
            {
                _contextInitialize = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            UnitDispose(disposing);
            base.Dispose(disposing);
        }

        public Guid EmpresaId
        {
            get
            {
                IEnumerable<string> values;

                // TODO: Resolver o problema quando o usuário envia duas vezes a plataforma fica no formato "plat1.fly01.com.br, plat1.fly01.com.br"
                if (Request.Headers.TryGetValues("EmpresaId", out values))
                    return Guid.Parse(values.FirstOrDefault());

                throw new ArgumentException("EmpresaId não informada.");
            }
        }

        public string AppUser
        {
            get
            {
                IEnumerable<string> values;

                if (Request.Headers.TryGetValues("AppUser", out values))
                    return values.FirstOrDefault();

                throw new ArgumentException("AppUser não informado.");
            }
        }

        internal virtual async Task<IHttpActionResult> Patch([FromODataUri] Guid key, TEntity entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Update(entity);
                await UnitSave();
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

        internal async Task<IHttpActionResult> Delete()
        {
            await UnitSave();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
