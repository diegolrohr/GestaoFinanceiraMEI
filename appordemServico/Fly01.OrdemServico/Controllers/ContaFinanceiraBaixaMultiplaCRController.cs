using Fly01.Core.Presentation;
using Fly01.uiJS.Classes;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroContasReceber)]
    public class ContaFinanceiraBaixaMultiplaCRController : ContaFinanceiraBaixaMultiplaController
    {
        public ContaFinanceiraBaixaMultiplaCRController()
            : base(ResourceHashConst.FinanceiroCadastrosContasBancarias) { }

        protected override ContentUI FormJson() 
            => FormBaixaMultipla("Receber");
    }
}