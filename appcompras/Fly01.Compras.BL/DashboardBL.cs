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


        public List<DashboardComprasVM> GetComprasStatus(DateTime filtro, string tipo)
        {
            List<DashboardComprasVM> quantidadeLista = new List<DashboardComprasVM>();
            List<DashboardComprasVM> retornos = new List<DashboardComprasVM>();
            this.filtro = filtro;
            //Retorna somente os dados de orçamentos de compras
            if (tipo == TipoOrdemCompra.Orcamento.ToString())
            {

                quantidadeLista = getQuantidadeStatus(TipoOrdemCompra.Orcamento);

                var groupJoin = (from orcamento in _orcamentoBL.All
                                 join ordemcompra in _ordemCompraBL.All on orcamento.Id equals ordemcompra.Id
                                 join orcamentoitem in _orcamentoItemBL.All on ordemcompra.Id equals orcamentoitem.OrcamentoId into ps
                                 from p in ps.DefaultIfEmpty()
                                 join ordemcompraitem in _ordemCompraItemBL.All on p.Id equals ordemcompraitem.Id into xs
                                 from x in xs.DefaultIfEmpty()
                                 where ordemcompra.Data.Month.Equals(filtro.Month) && ordemcompra.Data.Year.Equals(filtro.Year)
                                 select new
                                 {
                                     OrdemCompra = ordemcompra,
                                     OrdemCompraItem = x
                                 }).ToList();

                retornos = groupJoin.Select(x => new
                {
                    Total = x.OrdemCompraItem != null ? ((x.OrdemCompraItem.Quantidade * x.OrdemCompraItem.Valor) - x.OrdemCompraItem.Desconto) : 0,
                    Quantidade = x.OrdemCompraItem != null ? x.OrdemCompraItem.Quantidade : 0,
                    x.OrdemCompra.Status
                }).GroupBy(x => new { x.Status })
                    .Select(x => new DashboardComprasVM
                    {
                        Tipo = x.Key.Status.ToString(),
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


                    var groupJoin = (from pedido in _pedidoBL.All
                                     join ordemcompra in _ordemCompraBL.All on pedido.Id equals ordemcompra.Id
                                     join pedidoitem in _pedidoItemBL.All on ordemcompra.Id equals pedidoitem.PedidoId into ps
                                     from p in ps.DefaultIfEmpty()
                                     join ordemcompraitem in _ordemCompraItemBL.All on p.Id equals ordemcompraitem.Id into xs
                                     from x in xs.DefaultIfEmpty()
                                     where ordemcompra.Data.Month.Equals(filtro.Month) && ordemcompra.Data.Year.Equals(filtro.Year)
                                     select new
                                     {
                                         OrdemCompra = ordemcompra,
                                         Pedido = pedido,
                                         OrdemCompraItem = x
                                     }).ToList();

                    retornos = groupJoin.Select(x => new
                    {
                        Total = x.OrdemCompraItem != null ? x.Pedido.Total.GetValueOrDefault(0) : 0,
                        Quantidade = x.OrdemCompraItem != null ? x.OrdemCompraItem.Quantidade : 0,
                        x.OrdemCompra.Status
                    })
                    .GroupBy(x => new { x.Status })
                    .Select(x => new DashboardComprasVM
                    {
                        Tipo = x.Key.Status.ToString(),
                        Total = x.Sum(u => u.Total),
                        Quantidade = x.Count()
                    }).ToList();

                }
            }

            return (from x in retornos
                    join y in quantidadeLista on x.Tipo equals y.Tipo
                    select new DashboardComprasVM
                    { Tipo = x.Tipo, Quantidade = y.Quantidade, Total = x.Total }).ToList();
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


        public List<DashboardComprasVM> getComprasFormasPagamento(DateTime filtro, string tipo)
        {

            this.filtro = filtro;

            //Retorna somente os dados de orçamentos de compras
            if (tipo == TipoOrdemCompra.Orcamento.ToString())
            {
                List<DashboardComprasVM> quantidadeLista = getQuantidadeComprasPagamento(TipoOrdemCompra.Orcamento);

                var groupJoin = (from orcamento in _orcamentoBL.All
                                 join ordemcompra in _ordemCompraBL.All on orcamento.Id equals ordemcompra.Id
                                 join formapagamento in _formaPagamentoBL.All on ordemcompra.FormaPagamentoId equals formapagamento.Id into fs
                                 from f in fs.DefaultIfEmpty()
                                 join orcamentoitem in _orcamentoItemBL.All on ordemcompra.Id equals orcamentoitem.OrcamentoId into ps
                                 from p in ps.DefaultIfEmpty()
                                 join ordemcompraitem in _ordemCompraItemBL.All on p.Id equals ordemcompraitem.Id into xs
                                 from x in xs.DefaultIfEmpty()
                                 where orcamento.Data.Month.Equals(filtro.Month) && orcamento.Data.Year.Equals(filtro.Year)
                                 select new
                                 {
                                     OrdemCompra = ordemcompra,
                                     FormaPagamento = f,
                                     OrdemCompraItem = x
                                 }).ToList();

                List<DashboardComprasVM> orcamentos = groupJoin.Select(x => new
                {
                    Total = x.OrdemCompraItem != null ? ((x.OrdemCompraItem.Quantidade * x.OrdemCompraItem.Valor) - x.OrdemCompraItem.Desconto) : 0,
                    Quantidade = x.OrdemCompraItem != null ? x.OrdemCompraItem.Quantidade : 0,
                    Tipo = x.FormaPagamento != null ? x.FormaPagamento.TipoFormaPagamento.ToString() : ""
                })
                .GroupBy(x => new { x.Tipo })
                .Select(x => new DashboardComprasVM
                {
                    Tipo = x.Key.Tipo,
                    Total = Math.Round(x.Sum(u => u.Total), 2)
                }).ToList();

                List<DashboardComprasVM> listaComprasPagamento = new List<DashboardComprasVM>();

                foreach (var item in orcamentos)
                {
                    DashboardComprasVM itemCompras = new DashboardComprasVM();
                    itemCompras.Tipo = item.Tipo.Length == 0 ? "Não Definido" : EnumHelper.GetValue(typeof(TipoFormaPagamento), item.Tipo.ToString());                    
                    itemCompras.Total = item.Total;
                    listaComprasPagamento.Add(itemCompras);
                }

                return (from x in listaComprasPagamento
                        join y in quantidadeLista on x.Tipo equals y.Tipo
                        select new DashboardComprasVM
                        { Tipo = x.Tipo, Total = x.Total }).ToList();
            }
            else
            {
                //Retorna somente os dados de pedidos de compras
                if (tipo == TipoOrdemCompra.Pedido.ToString())
                {
                    List<DashboardComprasVM> quantidadeLista = getQuantidadeComprasPagamento(TipoOrdemCompra.Pedido);

                    var groupJoin = (from pedido in _pedidoBL.All
                                     join ordemcompra in _ordemCompraBL.All on pedido.Id equals ordemcompra.Id
                                     join formapagamento in _formaPagamentoBL.All on ordemcompra.FormaPagamentoId equals formapagamento.Id into fs
                                     from f in fs.DefaultIfEmpty()
                                     join pedidoitem in _pedidoItemBL.All on ordemcompra.Id equals pedidoitem.PedidoId into ps
                                     from p in ps.DefaultIfEmpty()
                                     join ordemcompraitem in _ordemCompraItemBL.All on p.Id equals ordemcompraitem.Id into xs
                                     from x in xs.DefaultIfEmpty()
                                     where pedido.Data.Month.Equals(filtro.Month) && pedido.Data.Year.Equals(filtro.Year)
                                     select new
                                     {
                                         OrdemCompra = ordemcompra,
                                         FormaPagamento = f,
                                         Pedido = pedido,
                                         OrdemCompraItem = x
                                     }).ToList();

                    List<DashboardComprasVM> pedidos = groupJoin.Select(x => new
                    {
                        //Total = x.OrdemCompraItem != null ? ((x.OrdemCompraItem.Quantidade * x.OrdemCompraItem.Valor) - x.OrdemCompraItem.Desconto) : 0,
                        Total = x.OrdemCompraItem != null ? x.Pedido.Total.GetValueOrDefault(0) : 0,
                        Quantidade = x.OrdemCompraItem != null ? x.OrdemCompraItem.Quantidade : 0,
                        Tipo = x.FormaPagamento != null ? x.FormaPagamento.TipoFormaPagamento.ToString() : ""
                    })
                    .GroupBy(x => new { x.Tipo })
                    .Select(x => new DashboardComprasVM
                    {
                        Tipo = x.Key.Tipo,
                        Total = x.Sum(u => u.Total)
                    }).ToList();

                    List<DashboardComprasVM> listaComprasPagamento = new List<DashboardComprasVM>();

                    foreach (var item in pedidos)
                    {
                        DashboardComprasVM itemCompras = new DashboardComprasVM();
                        itemCompras.Tipo = item.Tipo.Length == 0 ? "Não Definido" : EnumHelper.GetValue(typeof(TipoFormaPagamento), item.Tipo.ToString());
                        itemCompras.Total = item.Total;
                        listaComprasPagamento.Add(itemCompras);
                    }


                    return (from x in listaComprasPagamento
                            join y in quantidadeLista on x.Tipo equals y.Tipo
                            select new DashboardComprasVM
                            { Tipo = x.Tipo, Total = x.Total }).ToList();
                }
            }

            return null;
        }


        public List<DashboardComprasVM> getQuantidadeComprasPagamento(TipoOrdemCompra tipoOrdemCompra)
        {
            //Select realizado para mostrar a quantidade de orçamentos mesmo sem Itens
            var lista = _ordemCompraBL.All.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year) && x.TipoOrdemCompra == tipoOrdemCompra)
             .GroupJoin(_formaPagamentoBL.All, v =>
             v.FormaPagamentoId, p => p.Id, (ordemcompra, formapagamento) =>
             new { OrdemCompra = ordemcompra, FormaPagamento = formapagamento })
             .SelectMany(
             temp => temp.FormaPagamento.DefaultIfEmpty(),
             (temp, x) => new
             {
                 Tipo = x.TipoFormaPagamento.ToString()
             })
            .GroupBy(x => new { x.Tipo })
            .Select(x => new DashboardComprasVM { Tipo = x.Key.Tipo, Quantidade = x.Count() }).ToList();

            List<DashboardComprasVM> listaComprasPagamento = new List<DashboardComprasVM>();

            foreach (var item in lista)
            {
                DashboardComprasVM itemCompras = new DashboardComprasVM();
                itemCompras.Tipo = item.Tipo == null ? "Não Definido" : EnumHelper.GetValue(typeof(TipoFormaPagamento), item.Tipo.ToString());
                itemCompras.Total = item.Total;
                listaComprasPagamento.Add(itemCompras);
            }

            return listaComprasPagamento;
        }

        public List<DashboardComprasVM> getQuantidadeStatus(TipoOrdemCompra tipoOrdemCompra)
        {
            //Select realizado para mostrar a quantidade de orçamentos mesmo sem Itens
            return _ordemCompraBL.All.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year) && x.TipoOrdemCompra == tipoOrdemCompra)
                .Select(x => new { x.Status })
                .GroupBy(x => new { x.Status })
                .Select(x => new DashboardComprasVM
                {
                    Quantidade = x.Count(),
                    Tipo = x.Key.Status.ToString(),
                }).ToList();
        }
    }
}
