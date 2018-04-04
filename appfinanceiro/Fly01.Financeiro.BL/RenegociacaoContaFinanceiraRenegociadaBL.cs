using Fly01.Core.Api.BL;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Financeiro.Domain.Entities;

namespace Fly01.Financeiro.BL
{
    public class RenegociacaoContaFinanceiraRenegociadaBL : PlataformaBaseBL<RenegociacaoContaFinanceiraRenegociada>
    {
        public RenegociacaoContaFinanceiraRenegociadaBL(AppDataContext context) 
            : base(context) { }
    }
}
