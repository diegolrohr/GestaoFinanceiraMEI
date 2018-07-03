using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.BL;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Routing;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("arquivoremessa")]
    public class ArquivoRemessaController : ApiPlataformaController<ArquivoRemessa, ArquivoRemessaBL>
    {
        public override Task<IHttpActionResult> Post(ArquivoRemessa entity)
        {
            entity.DataExportacao = DateTime.Now;

            return base.Post(entity);
        }
    }
}