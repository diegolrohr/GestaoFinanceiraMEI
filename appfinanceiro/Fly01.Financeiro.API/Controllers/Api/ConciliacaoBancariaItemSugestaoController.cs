using System;
using System.Linq;
using System.Web.Http;
using Fly01.Core.Helpers;
using Fly01.Financeiro.BL;
using System.Collections.Generic;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;

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
                int totalCount = unitOfWork.ConciliacaoBancariaItemBL.All.Where(x => x.ConciliacaoBancariaId == conciliacaoBancariaId && x.StatusConciliado != StatusConciliado.Sim).Count();

                int skipRecords = (pageNo - 1) * pageSize;
                List<ConciliacaoBancariaItem> sugestoes = unitOfWork.ConciliacaoBancariaItemContaFinanceiraBL.GetConciliacaoBancariaItemSugestoes(conciliacaoBancariaId, skipRecords, pageSize);

                return Ok(new PagedResult<ConciliacaoBancariaItem>(sugestoes, pageNo, pageSize, totalCount));
            }
        }
    }
}
