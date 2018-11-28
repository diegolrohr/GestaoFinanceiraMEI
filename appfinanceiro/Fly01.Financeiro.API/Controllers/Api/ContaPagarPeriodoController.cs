using System;
using System.Linq;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("contapagarperiodo")]
    public class ContaPagarPeriodoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(
                    new
                    {
                        value = unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x =>
                            (x.DataInclusao >= dataInicial && x.DataInclusao <= dataFinal && x.ValorPago > 0) ||
                            (x.DataAlteracao >= dataInicial && x.DataAlteracao <= dataFinal && x.ValorPago > 0) ||
                            (x.DataExclusao >= dataInicial && x.DataExclusao <= dataFinal)
                        ).ToList()
                    }
                );
            }
        }
    }
}