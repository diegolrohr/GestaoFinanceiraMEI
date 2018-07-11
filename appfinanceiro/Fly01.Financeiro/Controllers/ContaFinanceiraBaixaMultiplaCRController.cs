using Fly01.Core.Presentation;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroContasReceber)]
    public class ContaFinanceiraBaixaMultiplaCRController : ContaFinanceiraBaixaMultiplaController
    {
        public ContaFinanceiraBaixaMultiplaCRController(string contaBancariaResourceHash)
            : base(ResourceHashConst.FinanceiroCadastrosContasBancarias) { }

        public override ContentResult Form() 
            => FormBaixaMultipla("Receber");
    }
}