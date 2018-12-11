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
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal, bool ignoraExclusao = false, int pageNumber = 1, int pageSize = 50)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var union =
                    unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x => x.DataInclusao >= dataInicial && x.DataInclusao <= dataFinal && x.ValorPago > 0).Union(
                    unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x => x.DataAlteracao >= dataInicial && x.DataAlteracao <= dataFinal && x.ValorPago > 0)).Union(
                    unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x => !ignoraExclusao && (x.DataExclusao >= dataInicial && x.DataExclusao <= dataFinal))).OrderBy(x => x.DataVencimento);

                int skipRecords = (pageNumber - 1) * pageSize;

                return Ok(
                    new
                    {
                        totalRecords = union.Count(),
                        totalPages = Math.Ceiling(((double)union.Count() / pageSize)),
                        value = union.Skip(skipRecords).Take(pageSize).ToList()
                    }
                );
            }
        }
    }
}