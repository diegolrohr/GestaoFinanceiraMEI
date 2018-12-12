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
                int skipRecords = (pageNumber - 1) * pageSize;

                var count =
                    unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                        x => x.Categoria,
                        x => x.FormaPagamento
                    ).Where(x =>
                        (x.DataAlteracao ?? x.DataInclusao) >= dataInicial &&
                        (x.DataAlteracao ?? x.DataInclusao) <= dataFinal &&
                        (x.ValorPago > 0)
                        ).Union(
                    unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                        x => x.Categoria,
                        x => x.FormaPagamento
                    ).Where(x => !ignoraExclusao && (x.DataExclusao >= dataInicial && x.DataExclusao <= dataFinal))).Count();

                var result =
                    unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                        x => x.Categoria,
                        x => x.FormaPagamento
                    ).Where(x =>
                        (x.DataAlteracao ?? x.DataInclusao) >= dataInicial &&
                        (x.DataAlteracao ?? x.DataInclusao) <= dataFinal &&
                        (x.ValorPago > 0)
                        ).Union(
                    unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                        x => x.Categoria,
                        x => x.FormaPagamento
                    ).Where(x => !ignoraExclusao && (x.DataExclusao >= dataInicial && x.DataExclusao <= dataFinal))).OrderBy(x => x.DataVencimento).Skip(skipRecords).Take(pageSize);

                return Ok(
                    new
                    {
                        totalRecords = count,
                        totalPages = Math.Ceiling(((double)count / (double)pageSize)),
                        value = result.ToList()
                    }
                );
            }
        }
    }
}