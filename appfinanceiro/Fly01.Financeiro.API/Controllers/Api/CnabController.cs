using Fly01.Financeiro.BL;
using System;
using System.Web.Http;
using Fly01.Core.API;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/cnab")]
    public class CnabController : ApiBaseController
    {
        [HttpGet]
        //public void ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId, DateTime dataDesconto, double valorDesconto)
        [Route("imprimeBoleto")]
        public IHttpActionResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            using (var unit = new UnitOfWork(ContextInitialize))
            {
                var boletos = unit.CnabBL.GeraBoletos(contaReceberId, contaBancariaId, DateTime.Now, 0);

                return Ok(boletos);
            }
        }
    }
}