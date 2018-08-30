using System;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("contapagarparcelas")]
    public class ContaPagarParcelasController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid contaFinanceiraParcelaPaiId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {                
                return Ok(new { value = unitOfWork.ContaPagarBL.GetParcelas(contaFinanceiraParcelaPaiId) });
            }
        }
    }
}