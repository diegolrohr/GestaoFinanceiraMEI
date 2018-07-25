using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class DashboardBL
    {
        private PedidoItemBL _pedidoItemBL;
        private OrdemCompraBL _ordemCompraBL;
        
        public DashboardBL(AppDataContext context, PedidoItemBL pedidoItemBL, OrdemCompraBL ordemcompraBL)
        {
            _pedidoItemBL = pedidoItemBL;
            _ordemCompraBL = ordemcompraBL;
        }

        public List<ProdutosMaisCompradosVM> GetProdutosMaisComprados(DateTime filtro)
        {
            return _pedidoItemBL
                .AllIncluding(x => x.Produto, x => x.Pedido)
                .Where(x => x.Pedido.Data.Month.Equals(filtro.Month) && x.Pedido.Data.Year.Equals(filtro.Year) && x.Pedido.Status == StatusOrdemCompra.Finalizado)
                .Select(x => new
                {
                    x.ProdutoId,
                    x.Produto.CodigoProduto,
                    x.Produto.Descricao,
                    x.Valor,
                    x.Quantidade,
                    UnidadeMedida = x.Produto.UnidadeMedida.Descricao
                })
                .GroupBy(x => new { x.CodigoProduto, x.Descricao, x.ProdutoId })
                .Select(x => new ProdutosMaisCompradosVM
                {
                    CodigoProduto = x.Key.CodigoProduto,
                    Descricao = x.Key.Descricao,
                    Quantidade = x.Sum(y => y.Quantidade),
                    Valor = x.Sum(y => y.Valor),
                    UnidadeMedida = x.Select(y => y.UnidadeMedida).FirstOrDefault()
                })
                .OrderByDescending(x => x.Quantidade)
                .Take(10).ToList();
        }
        public List<MaioresFornecedoresVM> GetMaioresFornecedores(DateTime filtro)
        {
            return _pedidoItemBL
                .AllIncluding(x => x.Pedido.Fornecedor)
                .Where(x => x.Pedido.Data.Month.Equals(filtro.Month) && x.Pedido.Data.Year.Equals(filtro.Year) && x.Pedido.Status == StatusOrdemCompra.Finalizado)
                .Select(x => new
                {
                    x.Pedido.Fornecedor.Id,
                    x.Pedido.Fornecedor.Nome,
                    x.Valor,
                    x.Quantidade
                })
                .GroupBy(x => new { x.Id, x.Nome })
                .Select(x => new MaioresFornecedoresVM
                {
                    Id = x.Key.Id.ToString(),
                    Nome = x.Key.Nome,
                    Valor = x.Sum(y => y.Valor * y.Quantidade)
                })
                .OrderByDescending(x => x.Valor)
                .Take(10).ToList();
        }

        public List<DashboardComprasVM> GetComprasStatus(DateTime filtro, string tipo)
        {
            return _ordemCompraBL.All
                .Where(x => x.TipoOrdemCompra.ToString() == tipo && x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year))
                .Select(x => new
                {
                    Descricao = x.Status.ToString(),
                    x.Total
                })
                .GroupBy(x => new { x.Descricao })
                .Select(x => new DashboardComprasVM
                {
                    Tipo = x.Key.Descricao,
                    Total = x.Sum(v => v.Total ?? 0)
                })
                .ToList();
        }
        public List<DashboardComprasVM> GetComprasCategoria(DateTime filtro, string tipo)
        {
            return _ordemCompraBL.AllIncluding(x => x.Categoria)
                .Where(x => x.TipoOrdemCompra.ToString() == tipo && x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year))
                .Select(x => new
                {
                    x.Categoria.Descricao,
                    x.Total
                })
                .GroupBy(x => new { x.Descricao })
                .Select(x => new DashboardComprasVM
                {
                    Tipo = x.Key.Descricao,
                    Total = x.Sum(v => v.Total ?? 0)
                })
                .ToList();
        }
        public List<DashboardComprasVM> GetComprasFormasPagamento(DateTime filtro, string tipo)
        {
            return _ordemCompraBL.AllIncluding(x => x.FormaPagamento)
                .Where(x => x.TipoOrdemCompra.ToString() == tipo && x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year))
                .Select(x => new
                {
                    x.FormaPagamento.Descricao,
                    x.Total
                })
                .GroupBy(x => new { x.Descricao })
                .Select(x => new DashboardComprasVM
                {
                    Tipo = x.Key.Descricao,
                    Total = x.Sum(v => v.Total ?? 0)
                })
                .ToList();
        }
    }
}
