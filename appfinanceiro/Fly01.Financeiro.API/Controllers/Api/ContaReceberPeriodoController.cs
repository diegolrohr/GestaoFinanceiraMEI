using System;
using System.Linq;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("contareceberperiodo")]
    public class ContaReceberPeriodoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal, bool ignoraExclusao = false, int pageNumber = 1, int pageSize = 50)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                int skipRecords = (pageNumber - 1) * pageSize;

                var count =
                    unitOfWork.ContaReceberBL.AllWithInactiveIncluding(
                        x => x.Categoria,
                        x => x.FormaPagamento
                    ).Where(x =>
                        (x.DataAlteracao ?? x.DataInclusao) >= dataInicial &&
                        (x.DataAlteracao ?? x.DataInclusao) <= dataFinal &&
                        (x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.Pago)
                        ).Union(
                    unitOfWork.ContaReceberBL.AllWithInactiveIncluding(
                        x => x.Categoria,
                        x => x.FormaPagamento
                    ).Where(x => !ignoraExclusao && (x.DataExclusao >= dataInicial && x.DataExclusao <= dataFinal))).Count();

                var result =
                    unitOfWork.ContaReceberBL.AllWithInactiveIncluding(
                        x => x.Categoria,
                        x => x.FormaPagamento
                    ).Where(x =>
                        (x.DataAlteracao ?? x.DataInclusao) >= dataInicial &&
                        (x.DataAlteracao ?? x.DataInclusao) <= dataFinal &&
                        (x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.Pago)
                        ).Union(
                    unitOfWork.ContaReceberBL.AllWithInactiveIncluding(
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