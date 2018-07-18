using Fly01.Core.Presentation;
using Fly01.uiJS.Classes;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroContasPagar)]
    public class ContaFinanceiraBaixaMultiplaCPController : ContaFinanceiraBaixaMultiplaController
    {
        public ContaFinanceiraBaixaMultiplaCPController() 
            : base(ResourceHashConst.FinanceiroCadastrosContasBancarias) { }

        protected override ContentUI FormJson() 
            => FormBaixaMultipla("Pagar");
    }
}