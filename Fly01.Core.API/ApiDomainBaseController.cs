using System;
using System.Linq;
using System.Web.Http;
using Fly01.Core.Api;
using System.Web.OData;
using Fly01.Core.Api.BL;
using Fly01.Core.Api.Domain;
using System.Web.OData.Routing;
using Fly01.Core.Base;

namespace Fly01.Core.API
{
    [CustomExceptionFilter]
    public abstract class ApiDomainBaseController<TEntity> : ApiODataBaseController<TEntity>
        where TEntity : DomainBase, new()
    {
        [EnableQuery(PageSize = 50, MaxTop = 50, MaxExpansionDepth = 10)]
        public virtual IHttpActionResult Get()
        {
            return Ok(All().Where(x => x.Ativo).AsQueryable());
        }

        [EnableQuery]
        [ODataRoute("({key})")]
        public virtual IHttpActionResult Get([FromODataUri] Guid key)
        {
            return Ok(SingleResult.Create(All().Where(x => x.Id == key && x.Ativo).AsQueryable()));
        }
    }
}