using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    public class ContaFinanceiraBaixaMultiplaCRController : ContaFinanceiraBaixaMultiplaController
    {
        public override ContentResult Form()
        {
            return FormBaixaMultipla("Receber");
        }

    }
}