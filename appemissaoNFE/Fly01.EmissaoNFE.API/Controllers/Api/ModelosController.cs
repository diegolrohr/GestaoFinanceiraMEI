using Fly01.EmissaoNFE.API.Model;
using Fly01.Core.Controllers.API;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("modelos")]
    public class ModelosController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new {
                Instrucao = "Importe as URLs das collections no seu Postman para ter acesso aos modelos",
                APIsTSS = AppDefault.CollectionTSS,
                APIsTributacao = AppDefault.CollectionTributacao
            });
        }
    }
}