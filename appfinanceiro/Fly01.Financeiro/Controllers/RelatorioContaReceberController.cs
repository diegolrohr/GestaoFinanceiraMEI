using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.uiJS.Classes;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroDashboardContasPagar)]
    public class RelatorioContaReceberController : RelatorioContaFinanceiraController
    {
        public RelatorioContaReceberController(): base("ContaReceber"){}

        protected override ContentUI FormRelatorioJson(UrlHelper url, string scheme)
        {
            var RelatorioJson = base.FormRelatorioJson(url, scheme);

            return RelatorioJson;
        }

    }

    
}
