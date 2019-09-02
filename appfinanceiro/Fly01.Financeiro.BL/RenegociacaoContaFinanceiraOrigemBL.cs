using Fly01.Core.BL;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class RenegociacaoContaFinanceiraOrigemBL : EmpresaBaseBL<RenegociacaoContaFinanceiraOrigem>
    {
        public RenegociacaoContaFinanceiraOrigemBL(AppDataContext context) 
            : base(context) { }
    }
}
