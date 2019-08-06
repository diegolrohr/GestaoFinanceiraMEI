using Fly01.Compras.ViewModel;
using Fly01.Core;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.uiJS.Classes;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNFeImportacao)]
    public class NFeImportacaoCobrancaController : BaseController<NFeImportacaoCobrancaVM>
    {
        public override Func<NFeImportacaoCobrancaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                numero = x.Numero
            };
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public JsonResult GetNFeImportacaoCobrancas(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "nFeImportacaoId eq", string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id }
            };
            return GridLoad(filters);
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}
