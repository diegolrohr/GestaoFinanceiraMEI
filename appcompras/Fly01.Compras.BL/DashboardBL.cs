using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class DashboardBL
    {
        private PedidoBL _pedidoBL;
        private PedidoItemBL _pedidoItemBL;
        private OrdemCompraBL _ordemCompraBL;
        
        public DashboardBL(AppDataContext context, PedidoBL pedidoBL, PedidoItemBL pedidoItemBL, OrdemCompraBL ordemcompraBL)
        {
            _pedidoBL = pedidoBL;
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
                    Desconto = (x.Desconto / x.Quantidade),
                    x.Valor,
                    x.Quantidade
                })
                .GroupBy(x => new { x.CodigoProduto, x.Descricao, x.ProdutoId, x.Desconto })
                .Select(x => new ProdutosMaisCompradosVM
                {
                    CodigoProduto = x.Key.CodigoProduto,
                    Descricao = x.Key.Descricao,
                    Quantidade = x.Sum(y => y.Quantidade),
                    Valor = x.Sum(y => y.Valor) - x.Key.Desconto
                })
                .OrderByDescending(x => x.Quantidade)
                .Take(10).ToList();
        }
        public List<MaioresFornecedoresVM> GetMaioresFornecedores(DateTime filtro)
        {
            return _pedidoBL
                .AllIncluding(x => x.Fornecedor)
                .Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year) && x.Status == StatusOrdemCompra.Finalizado)
                .Select(x => new 
                {
                    x.Fornecedor.Id,
                    x.Fornecedor.Nome,
                    x.Total
                })
                .GroupBy(x => new { x.Id, x.Nome })
                .Select(x => new MaioresFornecedoresVM
                {
                    Id = x.Key.Id,
                    Nome = x.Key.Nome,
                    Valor = x.Sum(y => y.Total)
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
                    Total = x.Sum(v => v.Total)
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
                    Tipo = x.Key.Descricao ?? "Não selecionada",
                    Total = x.Sum(v => v.Total)
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
                    Tipo = x.Key.Descricao ?? "Não selecionada",
                    Total = x.Sum(v => v.Total)
                })
                .ToList();
        }
    }
}
