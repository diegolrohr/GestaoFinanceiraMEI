using Fly01.Compras.DAL;
using Fly01.Compras.Domain.Entities;
using Fly01.Compras.Domain.Enums;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
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

        public OrcamentoBL(AppDataContext context, PedidoBL pedidoBL, OrcamentoItemBL orcamentoItemBL, PedidoItemBL pedidoItemBL, OrdemCompraBL ordemCompraBL) : base(context)
        {
            PedidoBL = pedidoBL;
            OrcamentoItemBL = orcamentoItemBL;
            PedidoItemBL = pedidoItemBL;
            OrdemCompraBL = ordemCompraBL;
        }

        public override void ValidaModel(Orcamento entity)
        {
            entity.Fail((entity.Status == StatusOrdemCompra.Finalizado && !OrcamentoItemBL.All.Any(x => x.OrcamentoId == entity.Id)), new Error("Para finalizar o orçamento é necessário ao menos ter adicionado um produto"));
            entity.Fail(entity.TipoOrdemCompra != TipoOrdemCompra.Orcamento, new Error("Permitido somente tipo orçamento"));
            entity.Fail(entity.Numero < 1, new Error("Numero do pedido menor que zero."));
            entity.Fail(All.Any(x => x.Numero == entity.Numero && x.Id != entity.Id), new Error("Numero do pedido repetido"));

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
                        CategoriaId = entity.CategoriaId,
                        DataVencimento = entity.DataVencimento,
                        TipoOrdemCompra = TipoOrdemCompra.Pedido,
                        TipoFrete = TipoFrete.SemFrete,
                        Observacao = entity.Observacao,
                        FornecedorId = fornecedorId,
                        OrcamentoOrigemId = entity.Id
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

        public override void Insert(Orcamento entity)
        {
            if (entity.Id == default(Guid))
            {
                entity.Id = Guid.NewGuid();
            }

            var max = OrdemCompraBL.Everything.Any(x => x.Id != entity.Id) ? OrdemCompraBL.Everything.Max(x => x.Numero) : 0;

            entity.Numero = (max == 1 && !OrdemCompraBL.Everything.Any(x => x.Id != entity.Id && x.Ativo && x.Numero == 1)) ? 1 : ++max;

            ValidaModel(entity);

            if (entity.Status == StatusOrdemCompra.Finalizado & entity.IsValid())
            {
                GeraPedidos(entity);
            }

            base.Insert(entity);
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
    }
}