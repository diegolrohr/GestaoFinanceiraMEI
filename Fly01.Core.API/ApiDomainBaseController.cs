using Fly01.Core.Entities.Domains;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

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