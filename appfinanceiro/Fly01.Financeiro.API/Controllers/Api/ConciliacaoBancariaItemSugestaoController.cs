using System;
using System.Linq;
using System.Web.Http;
using Fly01.Core.Helpers;
using Fly01.Financeiro.BL;
using System.Collections.Generic;
using Fly01.Core.Controllers.API;
using Fly01.Financeiro.Domain.Entities;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("conciliacaobancariaitemsugestao")]
    public class ConciliacaoBancariaItemSugestaoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid conciliacaoBancariaId, int pageNo = 1, int pageSize = 20)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                int skipRecords = (pageNo - 1) * pageSize;
                List<ConciliacaoBancariaItem> sugestoes = unitOfWork.ConciliacaoBancariaItemContaFinanceiraBL.GetConciliacaoBancariaItemSugestoes(conciliacaoBancariaId);

                int totalCount = sugestoes.Count;
                sugestoes = sugestoes.Skip(skipRecords).Take(pageSize).ToList();

                return Ok(new PagedResult<ConciliacaoBancariaItem>(sugestoes, pageNo, pageSize, totalCount));
            }
        }
    }
}
