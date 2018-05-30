using Fly01.Core.API;
using Fly01.Financeiro.BL;
using System;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/boleto")]
    public class BoletoController : ApiBaseController
    {
        [HttpGet]
        [Route("imprimeBoleto")]
        public IHttpActionResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.CnabBL.GetDadosBoleto(contaReceberId, contaBancariaId);

                return Ok(data);
            }
        }
    }
}