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
        private FormaPagamentoBL _formaPagamentoBL;
        private PedidoBL _pedidoBL;
        private PedidoItemBL _pedidoItemBL;
        private OrcamentoBL _orcamentoBL;
        private OrcamentoItemBL _orcamentoItemBL;
        private OrdemCompraBL _ordemCompraBL;
        private OrdemCompraItemBL _ordemCompraItemBL;
        private ProdutoBL _produtoBL;
        private DateTime filtro;

        public DashboardBL(AppDataContext context, FormaPagamentoBL formaPagamentoBL, PedidoBL pedidoBL, PedidoItemBL pedidoItemBL, OrcamentoBL orcamentoBL, OrcamentoItemBL orcamentoitemBL, OrdemCompraBL ordemcompraBL, OrdemCompraItemBL ordemCompraItemBL, ProdutoBL produtoBL)
        {
            this._formaPagamentoBL = formaPagamentoBL;
            this._pedidoBL = pedidoBL;
            this._pedidoItemBL = pedidoItemBL;
            this._orcamentoBL = orcamentoBL;
            this._orcamentoItemBL = orcamentoitemBL;
            this._ordemCompraBL = ordemcompraBL;
            this._ordemCompraItemBL = ordemCompraItemBL;
            this._produtoBL = produtoBL;
        }


        public List<ComprasStatusVM> GetComprasStatus(DateTime filtro, string tipo)
        {
            List<ComprasStatusVM> quantidadeLista = new List<ComprasStatusVM>();
            List<ComprasStatusVM> retornos = new List<ComprasStatusVM>();
            this.filtro = filtro;
            //Retorna somente os dados de orçamentos de compras
            if (tipo == TipoOrdemCompra.Orcamento.ToString())
            {

                quantidadeLista = getQuantidadeStatus(TipoOrdemCompra.Orcamento);

                retornos = _ordemCompraBL.All.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year))
                   .Join(_orcamentoBL.All, v =>
                   v.Id, p => p.Id, (ordemcompra, orcamento) =>
                   new { OrdemCompra = ordemcompra, Orcamento = orcamento })
                   .Join(_orcamentoItemBL.All, v =>
                   v.Orcamento.Id, p => p.OrcamentoId, (orcamento, orcamentoitem) =>
                   new { Orcamento = orcamento, OrcamentoItem = orcamentoitem })
                   .Join(_ordemCompraItemBL.All, v =>
                   v.OrcamentoItem.Id, p => p.Id, (orcamentoitem, ordemcompraitem) =>
                   new { Orcamentoitem = orcamentoitem, OrdemCompraItem = ordemcompraitem })
                   .Select(x => new
                   {
                       Total = ((x.OrdemCompraItem.Quantidade * x.OrdemCompraItem.Valor) - x.OrdemCompraItem.Desconto),
                       x.OrdemCompraItem.Quantidade,
                       x.Orcamentoitem.Orcamento.OrdemCompra.Status
                   })
                   .GroupBy(x => new { x.Status })
                   .Select(x => new ComprasStatusVM
                   {
                       Status = x.Key.Status.ToString(),
                       Total = Math.Round(x.Sum(u => u.Total), 2),
                       Quantidade = x.Count()
                   }).ToList();
            }
            else
            {
                //Retorna somente os dados de pedidos de compras
                if (tipo == TipoOrdemCompra.Pedido.ToString())
                {
                    quantidadeLista = getQuantidadeStatus(TipoOrdemCompra.Pedido);

                    retornos = _ordemCompraBL.All.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year))
                        .Join(_pedidoBL.All, v =>
                        v.Id, p => p.Id, (ordemcompra, pedido) =>
                        new { OrdemCompra = ordemcompra, Pedido = pedido })
                        .Join(_pedidoItemBL.All, v =>
                        v.Pedido.Id, p => p.PedidoId, (pedido, pedidoitem) =>
                        new { Pedido = pedido, PedidoItem = pedidoitem })
                        .Join(_ordemCompraItemBL.All, v =>
                        v.PedidoItem.Id, p => p.Id, (pedidoitem, ordemcompraitem) =>
                        new { PedidoItem = pedidoitem, OrdemCompraItem = ordemcompraitem })
                        .Select(x => new
                        {
                            Total = ((x.OrdemCompraItem.Quantidade * x.OrdemCompraItem.Valor) - x.OrdemCompraItem.Desconto),
                            x.OrdemCompraItem.Quantidade,
                            x.PedidoItem.Pedido.OrdemCompra.Status
                        })
                    .GroupBy(x => new { x.Status })
                    .Select(x => new ComprasStatusVM
                    {
                        Status = x.Key.Status.ToString(),
                        Total = Math.Round(x.Sum(u => u.Total), 2),
                        Quantidade = x.Count()
                    }).ToList();
                }
            }

            return (from x in retornos
                    join y in quantidadeLista on x.Status equals y.Status
                    select new ComprasStatusVM
                    { Status = x.Status, Quantidade = y.Quantidade, Total = x.Total }).ToList();
        }


        public List<ProdutosMaisCompradosVM> getProdutosMaisComprados(DateTime filtro)
        {
            return _ordemCompraItemBL.All.Where(x => x.DataInclusao.Month.Equals(filtro.Month) && x.DataInclusao.Year.Equals(filtro.Year))
                    .Join(_produtoBL.All, v =>
                    v.ProdutoId, p => p.Id, (ordemCompraItem, produto) =>
                    new { OrdemCompraItem = ordemCompraItem, Produto = produto })
                        .Select(x => new
                        {
                            x.OrdemCompraItem.ProdutoId,
                            x.Produto.CodigoProduto,
                            x.Produto.Descricao,
                            x.OrdemCompraItem.Valor,
                            x.OrdemCompraItem.Quantidade,
                            UnidadeMedida = x.Produto.UnidadeMedida.Descricao
                        })
                        .GroupBy(x => new { x.CodigoProduto, x.Descricao, x.ProdutoId })
                        .Select(x => new ProdutosMaisCompradosVM
                        {
                            CodigoProduto = x.Key.CodigoProduto,
                            Descricao = x.Key.Descricao,
                            Quantidade = x.Count(),
                            Valor = x.Sum(y => y.Valor),
                            UnidadeMedida = x.Select(y => y.UnidadeMedida).FirstOrDefault()
                        }).OrderByDescending(x => x.Quantidade).Take(10).ToList();
        }


        public List<ComprasFormasPagamentoVM> getComprasFormasPagamento(DateTime filtro, string tipo)
        {

            this.filtro = filtro;

            //Retorna somente os dados de orçamentos de compras
            if (tipo == TipoOrdemCompra.Orcamento.ToString())
            {
                List<ComprasFormasPagamentoVM> quantidadeLista = getQuantidadeComprasPagamento(TipoOrdemCompra.Orcamento);

                //Select realizado para mostrar o valores de orçamentos com Itens
                List<ComprasFormasPagamentoVM> orcamentos = _ordemCompraBL.All.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year))
                  .GroupJoin(_formaPagamentoBL.All, v =>
                  v.FormaPagamentoId, p => p.Id, (ordemcompra, formapagamento) =>
                  new { OrdemCompra = ordemcompra, FormaPagamento = formapagamento })
                  .Join(_orcamentoBL.All, v =>
                  v.OrdemCompra.Id, p => p.Id, (ordemcompra, orcamento) =>
                  new { OrdemCompra = ordemcompra, Orcamento = orcamento })
                 .Join(_orcamentoItemBL.All, v =>
                  v.Orcamento.Id, p => p.OrcamentoId, (orcamento, orcamentoitem) =>
                  new { Orcamento = orcamento, OrcamentoItem = orcamentoitem })
                  .Join(_ordemCompraItemBL.All, v =>
                  v.OrcamentoItem.Id, p => p.Id, (orcamentoitem, ordemcompraitem) =>
                  new { Orcamentoitem = orcamentoitem, OrdemCompraItem = ordemcompraitem })
                .SelectMany(
                temp => temp.Orcamentoitem.Orcamento.OrdemCompra.FormaPagamento.DefaultIfEmpty(),
                (temp, x) =>
                new
                {
                    Total = ((temp.OrdemCompraItem.Quantidade * temp.OrdemCompraItem.Valor) - temp.OrdemCompraItem.Desconto),
                    Quantidade = temp.OrdemCompraItem.Quantidade,
                    TipoFormaPagamento = x.TipoFormaPagamento.ToString()
                })
                .GroupBy(x => new { x.TipoFormaPagamento })
                .Select(x => new ComprasFormasPagamentoVM
                {
                    TipoFormaPagamento = x.Key.TipoFormaPagamento == "" ? "Não Definido" : x.Key.TipoFormaPagamento,
                    Total = Math.Round(x.Sum(u => u.Total), 2)
                }).ToList();

                return (from x in orcamentos
                        join y in quantidadeLista on x.TipoFormaPagamento equals y.TipoFormaPagamento
                        select new ComprasFormasPagamentoVM
                        { TipoFormaPagamento = x.TipoFormaPagamento, Quantidade = y.Quantidade, Total = x.Total }).ToList();
            }
            else
            {
                //Retorna somente os dados de pedidos de compras
                if (tipo == TipoOrdemCompra.Pedido.ToString())
                {

                    List<ComprasFormasPagamentoVM> quantidadeLista = getQuantidadeComprasPagamento(TipoOrdemCompra.Pedido);
                    List<ComprasFormasPagamentoVM> pedidos = _ordemCompraBL.All.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year) && x.TipoOrdemCompra == TipoOrdemCompra.Pedido)
                    .GroupJoin(_formaPagamentoBL.All, v =>
                    v.FormaPagamentoId, p => p.Id, (ordemcompra, formapagamento) =>
                    new { OrdemCompra = ordemcompra, FormaPagamento = formapagamento })
                    .Join(_pedidoBL.All, v =>
                    v.OrdemCompra.Id, p => p.Id, (ordemcompra, pedido) =>
                    new { OrdemCompra = ordemcompra, Pedido = pedido })
                   .Join(_pedidoItemBL.All, v =>
                    v.Pedido.Id, p => p.PedidoId, (pedido, pedidoItem) =>
                    new { Pedido = pedido, PedidoItem = pedidoItem })
                    .Join(_ordemCompraItemBL.All, v =>
                    v.PedidoItem.Id, p => p.Id, (pedidoItem, ordemcompraitem) =>
                    new { PedidoItem = pedidoItem, OrdemCompraItem = ordemcompraitem })
                    .SelectMany(
                    temp => temp.PedidoItem.Pedido.OrdemCompra.FormaPagamento.DefaultIfEmpty(),
                    (temp, x) =>
                    new
                    {
                        Total = ((temp.OrdemCompraItem.Quantidade * temp.OrdemCompraItem.Valor) - temp.OrdemCompraItem.Desconto),
                        temp.OrdemCompraItem.Quantidade,
                        TipoFormaPagamento = x.TipoFormaPagamento
                    })
                    .GroupBy(x => new { x.TipoFormaPagamento })
                    .Select(x => new ComprasFormasPagamentoVM
                    {
                        TipoFormaPagamento = x.Key.TipoFormaPagamento.ToString() == "" ? "Não Definido" : x.Key.TipoFormaPagamento.ToString(),
                        Total = Math.Round(x.Sum(u => u.Total), 2),
                        Quantidade = x.Count()
                    }).ToList();

                    return (from x in pedidos
                            join y in quantidadeLista on x.TipoFormaPagamento equals y.TipoFormaPagamento
                            select new ComprasFormasPagamentoVM
                            { TipoFormaPagamento = x.TipoFormaPagamento, Quantidade = y.Quantidade, Total = x.Total }).ToList();
                }
            }

            return null;
        }


        public List<ComprasFormasPagamentoVM> getQuantidadeComprasPagamento(TipoOrdemCompra tipoOrdemCompra)
        {
            //Select realizado para mostrar a quantidade de orçamentos mesmo sem Itens
            return _ordemCompraBL.All.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year) && x.TipoOrdemCompra == tipoOrdemCompra)
             .GroupJoin(_formaPagamentoBL.All, v =>
             v.FormaPagamentoId, p => p.Id, (ordemcompra, formapagamento) =>
             new { OrdemCompra = ordemcompra, FormaPagamento = formapagamento })
             .SelectMany(
             temp => temp.FormaPagamento.DefaultIfEmpty(),
             (temp, x) => new
             {
                 TipoFormaPagamento = x.TipoFormaPagamento.ToString()
             })
            .GroupBy(x => new { x.TipoFormaPagamento })
            .Select(x => new ComprasFormasPagamentoVM { TipoFormaPagamento = x.Key.TipoFormaPagamento.ToString() == "" ? "Não Definido" : x.Key.TipoFormaPagamento.ToString(), Quantidade = x.Count() }).ToList();
        }

        public List<ComprasStatusVM> getQuantidadeStatus(TipoOrdemCompra tipoOrdemCompra)
        {
            //Select realizado para mostrar a quantidade de orçamentos mesmo sem Itens
            return _ordemCompraBL.All.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year) && x.TipoOrdemCompra == tipoOrdemCompra)
                .Select(x => new { x.Status })
                .GroupBy(x => new { x.Status })
                .Select(x => new ComprasStatusVM
                {
                    Quantidade = x.Count(),
                    Status = x.Key.Status.ToString()
                }).ToList();
        }
    }
}
