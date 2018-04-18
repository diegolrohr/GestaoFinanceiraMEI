using System;
using System.Linq;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;

using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("renegociacaocontasrelacionamento")]
    public class RenegociacaoContasRelacionamentoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid contaFinanceiraId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                //se era filha e virou pai de outra renegociação, lista somente o relacionamento das contas geradas
                //por isso testa origem primeiro
                if(unitOfWork.RenegociacaoContaFinanceiraOrigemBL.All.Any(x => x.ContaFinanceiraId == contaFinanceiraId))
                {
                    //é conta origem, lista as renegociadas
                    var renegociacaoId = unitOfWork.RenegociacaoContaFinanceiraOrigemBL.All.Where(x => x.ContaFinanceiraId == contaFinanceiraId).FirstOrDefault().ContaFinanceiraRenegociacaoId;
                    var renegociacao = unitOfWork.ContaFinanceiraRenegociacaoBL.All.Where(x => x.Id == renegociacaoId).FirstOrDefault();

                    var recordsIds = (from c in unitOfWork.RenegociacaoContaFinanceiraRenegociadaBL.All
                                    where c.ContaFinanceiraRenegociacaoId == renegociacaoId
                                    select c.ContaFinanceiraId).ToList();

                    var result = unitOfWork.ContaFinanceiraBL.AllWithInactiveIncluding(x => x.FormaPagamento, x => x.Pessoa, x => x.Categoria).Where(x => recordsIds.Contains(x.Id)).ToList();

                    return Ok(new { renegociacao = renegociacao, value = result });
                }
                else if (unitOfWork.RenegociacaoContaFinanceiraRenegociadaBL.All.Any(x => x.ContaFinanceiraId == contaFinanceiraId))
                {
                    //é conta renegociada, lista as origens
                    var renegociacaoId = unitOfWork.RenegociacaoContaFinanceiraRenegociadaBL.All.Where(x => x.ContaFinanceiraId == contaFinanceiraId).FirstOrDefault().ContaFinanceiraRenegociacaoId;
                    var renegociacao = unitOfWork.ContaFinanceiraRenegociacaoBL.All.Where(x => x.Id == renegociacaoId).FirstOrDefault();
                    var recordsIds = (from c in unitOfWork.RenegociacaoContaFinanceiraOrigemBL.All
                                      where c.ContaFinanceiraRenegociacaoId == renegociacaoId
                                      select c.ContaFinanceiraId).ToList();

                    var result = unitOfWork.ContaFinanceiraBL.AllWithInactiveIncluding(x => x.FormaPagamento, x => x.Pessoa, x => x.Categoria).Where(x => recordsIds.Contains(x.Id)).ToList();

                    return Ok(new { renegociacao = renegociacao, value = result });
                }
                return NotFound();
            }
        }
    }
}