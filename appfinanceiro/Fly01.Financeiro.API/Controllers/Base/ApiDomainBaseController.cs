using Fly01.Financeiro.BL.Base;
using Fly01.Financeiro.Domain.Base;
using Fly01.Utils.Api;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Fly01.Financeiro.API.Controllers.Base
{
    [CustomExceptionFilter]
    public class ApiDomainBaseController<TEntity, TBL> : ApiODataBaseController<TEntity, TBL>
        where TEntity : DomainBase, new()
        where TBL : DomainBaseBL<TEntity>
    {        
        [EnableQuery(PageSize = 50, MaxTop = 50, MaxExpansionDepth = 10)]
        public virtual IHttpActionResult Get()
        {
            return Ok(unitOfWork.GetGenericBL<TBL>().All.Where(x => x.Ativo).AsQueryable());
        }

        [EnableQuery]
        [ODataRoute("({key})")]
        public virtual IHttpActionResult Get([FromODataUri] Guid key)
        { 
            return Ok(SingleResult.Create(unitOfWork.GetGenericBL<TBL>().All.
                Where(x => x.Id == key &&
                           x.Ativo).AsQueryable()));
        }
    }
}