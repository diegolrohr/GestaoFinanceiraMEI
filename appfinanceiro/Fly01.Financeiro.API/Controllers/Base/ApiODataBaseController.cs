using Fly01.Financeiro.BL;
using Fly01.Financeiro.BL.Base;
using Fly01.Financeiro.Domain.Base;
using Fly01.Utils.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using Fly01.Financeiro.Domain;

namespace Fly01.Financeiro.API.Controllers.Base
{
    [CustomExceptionFilter]
    public class ApiODataBaseController<TEntity, TBL> : ODataController
        where TEntity : DomainBase, new()
        where TBL : DomainBaseBL<TEntity>
    {
        protected UnitOfWorkGeneric<TEntity> _unitOfWork = null;
        protected UnitOfWorkGeneric<TEntity> unitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                    _unitOfWork = new UnitOfWorkGeneric<TEntity>(new UnitOfWorkInitialize() { PlatformUrl = PlataformaUrl, UserName = UserName });
                return _unitOfWork;
            }
        }

        public ApiODataBaseController()
        {

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (unitOfWork != null)
                {
                    unitOfWork.Dispose();
                }
                _unitOfWork = null;
            }
            base.Dispose(disposing);
        }

        public string PlataformaUrl
        {
            get
            {
                IEnumerable<string> values;
                if (Request.Headers.TryGetValues("PlataformaUrl", out values))
                    return values.FirstOrDefault();

                throw new ArgumentException("PlataformaUrl não informada.");
            }
        }

        public string UserName
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
            {
                return BadRequest(ModelState);
            }

            try
            {
                unitOfWork.GetGenericBL<TBL>().Update(entity);
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

        internal async Task<IHttpActionResult> Delete()
        {
            await unitOfWork.Save(PlataformaUrl, UserName);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
