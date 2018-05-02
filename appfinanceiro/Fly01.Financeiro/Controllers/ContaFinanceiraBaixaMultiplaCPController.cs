using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    public class ContaFinanceiraBaixaMultiplaCPController : ContaFinanceiraBaixaMultiplaController
    {
        public override ContentResult Form()
        {
            return FormBaixaMultipla("Pagar");
        }

    }
}