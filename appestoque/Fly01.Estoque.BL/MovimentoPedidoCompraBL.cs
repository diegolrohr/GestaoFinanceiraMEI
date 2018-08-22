using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Estoque.BL
{
    public class MovimentoPedidoCompraBL : PlataformaBaseBL<MovimentoPedidoCompra>
    {
        protected ProdutoBL ProdutoBL;
        protected MovimentoEstoqueBL MovimentoEstoqueBL;

        public MovimentoPedidoCompraBL(AppDataContextBase context, ProdutoBL produtoBL, MovimentoEstoqueBL movimentoEstoqueBL)
            : base(context)
        {
            ProdutoBL = produtoBL;
            MovimentoEstoqueBL = movimentoEstoqueBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void Insert(MovimentoPedidoCompra entity)
        {
            var produto = ProdutoBL.All.FirstOrDefault(x => x.Id == entity.ProdutoId);

            if (produto == null)
            {
                return;
            }
            else
            {
                //compra devolução é saída.
                if (entity.TipoCompra == TipoVenda.Devolucao)
                {
                    var diferenca = produto.SaldoProduto - entity.Quantidade;
                    //se vai ficar negativo, dar entrada automática da diferenca antes
                    if (diferenca < 0)
                    {
                        MovimentoEstoque movimentoEntrada = new MovimentoEstoque()
                        {
                            QuantidadeMovimento = -diferenca,
                            ProdutoId = entity.ProdutoId,
                            Observacao = @"Observação gerada pela entrada do estoque automática, evitando estoque negativo, referente ao pedido nº " + entity.PedidoNumero.ToString() + ", aplicativo Bemacash Faturamento",
                            UsuarioInclusao = entity.UsuarioInclusao,
                            PlataformaId = entity.PlataformaId
                        };
                        MovimentoEstoqueBL.Movimenta(movimentoEntrada);
                    }
                }
                //compra normal é entrada
                if (entity.TipoCompra == TipoVenda.Normal)
                {
                    MovimentoEstoque movimentoEntrada = new MovimentoEstoque()
                    {
                        QuantidadeMovimento = (entity.TipoCompra == TipoVenda.Devolucao) ? -entity.Quantidade : entity.Quantidade,
                        ProdutoId = entity.ProdutoId,
                        Observacao = @"Observação gerada pela movimentação do estoque, referente ao pedido nº " + entity.PedidoNumero.ToString() + ", aplicativo Bemacash Compras",
                        UsuarioInclusao = entity.UsuarioInclusao,
                        PlataformaId = entity.PlataformaId
                    };
                    MovimentoEstoqueBL.Movimenta(movimentoEntrada);
                }
            }
        }

        public override void Update(MovimentoPedidoCompra entity)
        {
            throw new BusinessException("Não é possível alterar um movimento de baixa");
        }

        public override void Delete(MovimentoPedidoCompra entityToDelete)
        {
            throw new BusinessException("Não é possível excluir um movimento de baixa");
        }

        public static Error ProdutoNaoInformado = new Error("O produto não foi informado ou o Id é inválido.", "produtoId");
    }
}