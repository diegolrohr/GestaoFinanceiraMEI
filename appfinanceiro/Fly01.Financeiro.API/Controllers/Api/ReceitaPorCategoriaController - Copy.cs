using System;
using System.Web.Http;
using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Utils.Controllers.API;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("movimentacaoporcategoria")]
    public class MovimentacaoPorCategoriaController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.MovimentacaoPorCategoriaBL.Get(dataInicial, dataFinal);
                return Ok(new { value = data });
            }
        }
    }
}