using Fly01.Core.Presentation;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Estoque.ViewModel;
using Fly01.uiJS.Classes;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.EstoqueEstoqueInventario)]
    public class InventarioItemController : BaseController<InventarioItemVM>
    {
        public InventarioItemController()
        {
            ExpandProperties = "produto($select=descricao,codigoProduto,valorCusto,saldoProduto,unidadeMedidaId),produto($expand=unidadeMedida)";
        }

        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override Func<InventarioItemVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                codigoProduto = x.Produto.CodigoProduto,
                descricaoProduto = x.Produto.Descricao,
                unidadeMedida = x.Produto.UnidadeMedida.Abreviacao,
                custoProduto = x.Produto.ValorCusto,
                saldoEstoque = x.Produto.SaldoProduto,
                saldoInventariado = x.SaldoInventariado,
                produtoIdRow = x.ProdutoId,
                inventarioIdRow = x.InventarioId
            };
        }

        public override ContentResult List() { throw new NotImplementedException(); }

        public virtual JsonResult GridLoadInventarioItem(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "inventarioId eq", string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id }
            };
            return GridLoad(filters);
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}