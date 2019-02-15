using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroDashboardContasPagar)]
    public class RelatorioCentroCustoCPController : RelatorioCentroCustoController
    {
        public RelatorioCentroCustoCPController(): base("ContaPagar"){}

        protected override ContentUI FormRelatorioJson(UrlHelper url, string scheme)
        {
            var RelatorioJson = base.FormRelatorioJson(url, scheme);

            return RelatorioJson;
        }
    }

    
}
