using Fly01.Core.Presentation;
using Fly01.uiJS.Classes;
using System.Web.Http;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
    public class ContaFinanceiraBaixaMultiplaCPController : ContaFinanceiraBaixaMultiplaController
    {
        public ContaFinanceiraBaixaMultiplaCPController() 
            : base() { }

        protected override ContentUI FormJson() 
            => FormBaixaMultipla("Pagar");
    }
}