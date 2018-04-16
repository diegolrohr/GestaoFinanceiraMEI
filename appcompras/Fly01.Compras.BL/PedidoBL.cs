using Fly01.Compras.DAL;
using Fly01.Compras.Domain.Entities;
using Fly01.Compras.Domain.Enums;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class PedidoBL : PlataformaBaseBL<Pedido>
    {
        protected PedidoItemBL PedidoItemBL { get; set; }
        protected OrdemCompraBL OrdemCompraBL { get; set; }
        private readonly string descricaoPedido = @"Pedido nº: {0}";
        private readonly string observacaoPedido = @"Observação gerada pelo Pedido nº {0} applicativo Fly01 Compras: {1}";
        private readonly string routePrefixNameContaPagar = @"ContaPagar";
        private readonly string routePrefixNameMovimento = @"Movimento";
        private StatusOrdemCompra previousStatus;

        public PedidoBL(AppDataContext context, PedidoItemBL pedidoItemBL, OrdemCompraBL ordemCompraBL) : base(context)
        {
            PedidoItemBL = pedidoItemBL;
            OrdemCompraBL = ordemCompraBL;
        }

        public override void ValidaModel(Pedido entity)
        {
            entity.Fail(entity.TipoOrdemCompra != TipoOrdemCompra.Pedido, new Error("Permitido somente tipo pedido"));
            entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
            entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
            entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            entity.Fail(entity.QuantidadeVolumes.HasValue && entity.QuantidadeVolumes.Value < 0, new Error("Quantidade de volumes não pode ser negativo", "quantidadeVolumes"));
            entity.Fail(entity.Numero < 1, new Error("Numero do pedido menor que zero."));
            entity.Fail(All.Any(x => x.Numero == entity.Numero && x.Id != entity.Id), new Error("Numero do pedido repetido"));

            if (entity.Status == StatusOrdemCompra.Finalizado)
            {
                entity.Fail(!PedidoItemBL.All.Any(x => x.PedidoId == entity.Id), new Error("Para finalizar o pedido é necessário ao menos ter adicionado um produto"));
                entity.Fail(
                    (entity.GeraFinanceiro && (entity.FormaPagamentoId == null || entity.CondicaoParcelamentoId == null || entity.CategoriaId == null || entity.DataVencimento == null)),
                    new Error("Pedido que gera financeiro é necessário informar forma de pagamento, condição de parcelamento, categoria e data vencimento")
                    );
            }

            base.ValidaModel(entity);
        }

        public override void Update(Pedido entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            previousStatus = previous.Status;

            entity.Fail(previous.Status != StatusOrdemCompra.Aberto && entity.Status != StatusOrdemCompra.Aberto, new Error("Somente pedido em aberto pode ser alterado", "status"));

            if ((previous.Status == StatusOrdemCompra.Aberto && entity.Status == StatusOrdemCompra.Finalizado) && entity.IsValid())

                base.Update(entity);
        }

        public override void Delete(Pedido entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != StatusOrdemCompra.Aberto, new Error("Somente pedido em aberto pode ser deletado", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }

        public override void Insert(Pedido entity)
        {
            var max = OrdemCompraBL.Everything.Any(x => x.Id != entity.Id) ? OrdemCompraBL.Everything.Max(x => x.Numero) : 0;

            entity.Numero = (max == 1 && !OrdemCompraBL.Everything.Any(x => x.Id != entity.Id && x.Ativo && x.Numero == 1)) ? 1 : ++max;

            ValidaModel(entity);

            base.Insert(entity);
        }

        public override void AfterSave(Pedido entity)
        {
            if (entity.Status != StatusOrdemCompra.Finalizado)
                return;
            else if (previousStatus != StatusOrdemCompra.Aberto)
                return;

            var pedidoItens = PedidoItemBL.All.Where(e => e.PedidoId == entity.Id && e.Ativo);

            if (entity.GeraFinanceiro)
            {
                double total = pedidoItens.Select(i => (i.Quantidade * i.Valor) - i.Desconto).Sum();
                double valorPrevisto = total + ((entity.TipoFrete == TipoFrete.FOB || entity.TipoFrete == TipoFrete.Destinatario) ? entity.ValorFrete.Value : 0);

                ContaPagar contaPagar = new ContaPagar()
                {
                    ValorPrevisto = valorPrevisto,
                    CategoriaId = entity.CategoriaId.Value,
                    CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                    PessoaId = entity.FornecedorId,
                    DataEmissao = entity.Data,
                    DataVencimento = entity.DataVencimento.Value,
                    Descricao = string.Format(descricaoPedido, entity.Numero),
                    Observacao = string.Format(observacaoPedido, entity.Numero, entity.Observacao),
                    FormaPagamentoId = entity.FormaPagamentoId.Value,
                    PlataformaId = PlataformaUrl,
                    UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
                };

                Producer.Send(routePrefixNameContaPagar, contaPagar, RabbitConfig.enHTTPVerb.POST);
            }

            if (entity.MovimentaEstoque)
            {
                foreach (var item in pedidoItens)
                {
                    Movimento movimento = new Movimento()
                    {
                        QuantidadeMovimento = item.Quantidade,
                        ProdutoId = item.ProdutoId,
                        UsuarioInclusao = entity.UsuarioInclusao,
                        PlataformaId = PlataformaUrl
                    };

                    Producer.Send(routePrefixNameMovimento, movimento, RabbitConfig.enHTTPVerb.POST);
                }
            }
        }
    }
}