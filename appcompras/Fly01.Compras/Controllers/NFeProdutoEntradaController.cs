using Fly01.Compras.ViewModel;
using Fly01.Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.uiJS.Classes;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasNotasFiscais)]
    public class NFeProdutoEntradaController : BaseController<NFeProdutoEntradaVM>
    {
        public NFeProdutoEntradaController()
        {
            ExpandProperties = "produto($select=descricao),grupoTributario($select=descricao)";
        }

        public override Func<NFeProdutoEntradaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                produto_descricao = x.Produto.Descricao,
                grupoTributario_descricao = x.GrupoTributario.Descricao,
                quantidade = x.Quantidade.ToString("N", AppDefaults.CultureInfoDefault),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                desconto = x.Desconto.ToString("C", AppDefaults.CultureInfoDefault),
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
            };
        }

        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override ContentResult List() { throw new NotImplementedException(); }

        public JsonResult GetNFeProdutos(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "notaFiscalEntradaId eq", string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id }
            };
            return GridLoad(filters);
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}