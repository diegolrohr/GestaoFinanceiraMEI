using Fly01.Estoque.BL;
using Fly01.Core.Controllers.API;
using System;
using System.Web.Http;

namespace Fly01.Estoque.API.Controllers.Api
{
    [RoutePrefix("produtosmenosmovimentados")]
    public class ProdutosMenosMovimentadosController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.ProdutosMenosMovimentadosBL.Get(dataInicial, dataFinal);
                return Ok(new { value = data });
            }
        }
    }
}