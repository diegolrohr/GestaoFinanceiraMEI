using Fly01.Compras.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class OrcamentoBL : PlataformaBaseBL<Orcamento>
    {
        protected PedidoBL PedidoBL { get; set; }
        protected OrcamentoItemBL OrcamentoItemBL { get; set; }
        protected PedidoItemBL PedidoItemBL { get; set; }
        protected OrdemCompraBL OrdemCompraBL { get; set; }
        protected KitItemBL KitItemBL { get; set; }
        protected EstadoBL EstadoBL { get; set; }

        public OrcamentoBL(AppDataContext context, PedidoBL pedidoBL, OrcamentoItemBL orcamentoItemBL, PedidoItemBL pedidoItemBL, OrdemCompraBL ordemCompraBL, KitItemBL kitItemBL, EstadoBL estadoBL) : base(context)
        {
            PedidoBL = pedidoBL;
            OrcamentoItemBL = orcamentoItemBL;
            PedidoItemBL = pedidoItemBL;
            OrdemCompraBL = ordemCompraBL;
            KitItemBL = kitItemBL;
            EstadoBL = estadoBL;
        }

        public override void ValidaModel(Orcamento entity)
        {
            entity.Fail((entity.Status == StatusOrdemCompra.Finalizado && !OrcamentoItemBL.All.Any(x => x.OrcamentoId == entity.Id)), new Error("Para finalizar o orçamento é necessário ao menos ter adicionado um produto"));
            entity.Fail(entity.TipoOrdemCompra != TipoOrdemCompra.Orcamento, new Error("Permitido somente tipo orçamento"));
            entity.Fail(entity.Numero < 1, new Error("Numero do orçamento inválido"));
            entity.Fail(All.Any(x => x.Numero == entity.Numero && x.Id != entity.Id && x.Ativo), new Error("Numero do orçamento já foi utilizado"));

            base.ValidaModel(entity);
        }

        protected void GeraPedidos(Orcamento entity)
        {
            //gerar pedidos divididos por fornecedor
            var itens = OrcamentoItemBL.All.Where(x => x.OrcamentoId == entity.Id);
            if (itens != null)
            {
                var fornecedoresIds = itens.Select(x => x.FornecedorId).Distinct().ToList();

                foreach (Guid fornecedorId in fornecedoresIds)
                {
                    //a cada iteração new() para limpar a lista
                    var orcamentoItens = new List<OrcamentoItem>();
                    orcamentoItens = itens.Where(x => x.FornecedorId == fornecedorId).ToList();

                    var pedidoId = Guid.NewGuid();

                    var pedido = new Pedido()
                    {
                        Id = pedidoId,
                        Status = StatusOrdemCompra.Aberto,
                        Data = entity.Data,
                        FormaPagamentoId = entity.FormaPagamentoId,
                        CondicaoParcelamentoId = entity.CondicaoParcelamentoId,
                        CentroCustoId = entity.CentroCustoId,
                        CategoriaId = entity.CategoriaId,
                        DataVencimento = entity.DataVencimento,
                        TipoOrdemCompra = TipoOrdemCompra.Pedido,
                        TipoFrete = TipoFrete.SemFrete,
                        Observacao = entity.Observacao,
                        FornecedorId = fornecedorId,
                        OrcamentoOrigemId = entity.Id,
                        TipoCompra = TipoCompraVenda.Normal,
                        Total = orcamentoItens.Sum(x => x.Total),
                        PlataformaId = PlataformaUrl.ToString()
                    };
                    PedidoBL.Insert(pedido);

                    var pedidoItens = new List<PedidoItem>();
                    pedidoItens = orcamentoItens.Select(
                            x => new PedidoItem
                            {
                                PedidoId = pedidoId,
                                ProdutoId = x.ProdutoId,
                                Quantidade = x.Quantidade,
                                Valor = x.Valor,
                                Desconto = x.Desconto,
                            }).ToList();

                    foreach (var pedidoItem in pedidoItens)
                    {
                        PedidoItemBL.Insert(pedidoItem);
                    }
                }
            }
            else
            {
                throw new BusinessException("Para finalizar e gerar pedidos, o orçamento é necessário ao menos ter adicionado um produto");
            }
        }

        public IQueryable<OrdemCompra> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        public override void Insert(Orcamento entity)
        {
            if (entity.Id == default(Guid))
            {
                entity.Id = Guid.NewGuid();
            }

            var numero = default(int);
            rpc = new RpcClient();
            numero = int.Parse(rpc.Call($"plataformaid={PlataformaUrl},tipoordemcompra={(int)TipoOrdemCompra.Pedido}"));
            //numero = All.Max(x => x.Numero) + 1;
            entity.Numero = numero;
            ValidaModel(entity);

            if (entity.Status == StatusOrdemCompra.Finalizado & entity.IsValid())
            {
                GeraPedidos(entity);
            }

            GetIdPlacaEstado(entity);
            base.Insert(entity);
        }

        public void GetIdPlacaEstado(Orcamento entity)
        {
            if (!entity.EstadoPlacaVeiculoId.HasValue && !string.IsNullOrEmpty(entity.EstadoCodigoIbge))
            {
                var dadosEstado = EstadoBL.All.FirstOrDefault(x => x.CodigoIbge == entity.EstadoCodigoIbge);
                if (dadosEstado != null)
                {
                    entity.EstadoPlacaVeiculoId = dadosEstado.Id;
                }
            }
        }

        public override void Update(Orcamento entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            entity.Fail(previous.Status != StatusOrdemCompra.Aberto && entity.Status != StatusOrdemCompra.Aberto, new Error("Somente orçamento em aberto pode ser alterado", "status"));

            ValidaModel(entity);

 
            if (entity.Status == StatusOrdemCompra.Finalizado & entity.IsValid())
            {
                GeraPedidos(entity);
            }

            base.Update(entity);
        }

        public override void Delete(Orcamento entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != StatusOrdemCompra.Aberto, new Error("Somente orçamento em aberto pode ser deletado", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }

        public void UtilizarKitOrcamento(UtilizarKitVM entity)
        {
            try
            {
                if (All.Any(x => x.Id == entity.OrcamentoPedidoId))
                {
                    if (KitItemBL.All.Any(x => x.KitId == entity.KitId))
                    {
                        #region Produtos
                        if (entity.AdicionarProdutos)
                        {
                            var kitProdutos = KitItemBL.All.Where(x => x.KitId == entity.KitId && x.TipoItem == TipoItem.Produto);

                            var existentesOrcamento =
                                from oi in OrcamentoItemBL.AllIncluding(x => x.Produto).Where(x => x.OrcamentoId == entity.OrcamentoPedidoId)
                                join ki in kitProdutos on oi.ProdutoId equals ki.ProdutoId
                                select new { ProdutoId = ki.ProdutoId, OrcamentoItemId = oi.Id, Quantidade = ki.Quantidade };

                            var novasOrcamentoItens =
                                from kit in kitProdutos
                                where !existentesOrcamento.Select(x => x.ProdutoId).Contains(kit.ProdutoId)
                                select new
                                {
                                    OrcamentoId = entity.OrcamentoPedidoId,
                                    ProdutoId = kit.ProdutoId.Value,
                                    Valor = kit.Produto.ValorVenda,
                                    Quantidade = kit.Quantidade,
                                    FornecedorId = entity.FornecedorPadraoId
                                };

                            foreach (var item in novasOrcamentoItens)
                            {
                                OrcamentoItemBL.Insert(new OrcamentoItem()
                                {
                                    FornecedorId = item.FornecedorId.Value,
                                    ProdutoId = item.ProdutoId,
                                    OrcamentoId = item.OrcamentoId,
                                    Valor = item.Valor,
                                    Quantidade = item.Quantidade
                                });
                            }

                            if (entity.SomarExistentes)
                            {
                                foreach (var item in existentesOrcamento)
                                {
                                    var orcamentoItem = OrcamentoItemBL.Find(item.OrcamentoItemId);
                                    orcamentoItem.Quantidade += item.Quantidade;
                                    OrcamentoItemBL.Update(orcamentoItem);
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}