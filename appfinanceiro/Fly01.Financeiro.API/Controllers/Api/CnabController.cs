using Fly01.Financeiro.BL;
using System;
using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/cnab")]
    public class CnabController : ApiBaseController
    {
        [HttpGet]
        [Route("imprimeBoleto")]
        public IHttpActionResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                //return Ok(unitOfWork.CnabBL.GeraBoletos(contaReceberId, contaBancariaId, DateTime.Now, 0));
                return Ok(unitOfWork.CnabBL.GetDadosBoleto(contaReceberId, contaBancariaId));
            }
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(new { value = unitOfWork.CnabBL.Get() });
            }
        }

        [HttpGet]
        [Route("contasReceberarquivo")]
        public IHttpActionResult GetContasReceber(Guid IdArquivoRemessa)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(new { value = unitOfWork.CnabBL.GetContasReceberArquivo(IdArquivoRemessa)});
            }
        }
    }
}