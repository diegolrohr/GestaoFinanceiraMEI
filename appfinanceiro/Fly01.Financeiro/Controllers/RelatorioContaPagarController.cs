﻿using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.uiJS.Classes;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroRelatorios)]
    public class RelatorioContaPagarController : RelatorioContaFinanceiraController
    {
        public RelatorioContaPagarController(): base("ContaPagar"){}

        protected override ContentUI FormRelatorioJson(UrlHelper url, string scheme)
        {
            var RelatorioJson = base.FormRelatorioJson(url, scheme);

            return RelatorioJson;
        }
    }
}
