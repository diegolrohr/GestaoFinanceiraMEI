using Fly01.Core.Presentation;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroContasPagar)]
    public class ContaFinanceiraBaixaMultiplaCPController : ContaFinanceiraBaixaMultiplaController
    {
        public ContaFinanceiraBaixaMultiplaCPController() 
            : base(ResourceHashConst.FinanceiroCadastrosContasBancarias) { }

        public override ContentResult Form() 
            => FormBaixaMultipla("Pagar");
    }
}