using System.Collections.Generic;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Financeiro.Controllers
{
    public class FormaPagamentoController : FormaPagamentoBaseController<FormaPagamentoVM>
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$select", "id,descricao,tipoFormaPagamento,registroFixo");

            return customFilters;
        }
    }
}