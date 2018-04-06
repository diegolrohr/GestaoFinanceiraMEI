using Fly01.Financeiro.BL;
using Fly01.Core.API;
using System;
using System.Web.Http;
using System.Web.OData.Routing;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("movimentacaoporcategoria")]
    public class MovimentacaoPorCategoriaController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataInicial,
                                     DateTime dataFinal,
                                     bool somaRealizados = true,
                                     bool somaPrevistos = false)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.MovimentacaoPorCategoriaBL.Get(dataInicial, dataFinal, somaRealizados, somaPrevistos);
                return Ok(new { value = data });
            }
        }
    }
}