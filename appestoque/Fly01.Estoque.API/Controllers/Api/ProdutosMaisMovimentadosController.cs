using Fly01.Core.API;
using Fly01.Estoque.BL;
using System;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.API.Controllers.Api
{
    [RoutePrefix("produtosmaismovimentados")]
    public class ProdutosMaisMovimentadosController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.ProdutosMaisMovimentadosBL.Get(dataInicial, dataFinal);
                return Ok(new { value = data });
            }
        }
    }
}