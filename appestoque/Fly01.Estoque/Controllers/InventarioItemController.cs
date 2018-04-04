﻿using Fly01.Estoque.Controllers.Base;
using Fly01.Estoque.Entities.ViewModel;
using Fly01.Core;
using Fly01.Core.Api;
using Fly01.Core.Helpers;
using Fly01.Core.JQueryDataTable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    public class InventarioItemController : BaseController<InventarioItemVM>
    {

        public InventarioItemController()
        {
            ExpandProperties = "produto($select=descricao,codigoProduto,valorCusto,saldoProduto,unidadeMedidaId),produto($expand=unidadeMedida)";
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

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

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public virtual JsonResult GridLoadInventarioItem(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "inventarioId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id }
            };
            return GridLoad(filters);
        }
    }
}