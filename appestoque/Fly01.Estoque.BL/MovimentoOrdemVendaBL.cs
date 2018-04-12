using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Estoque.Domain.Entities;

namespace Fly01.Estoque.BL
{
    public class MovimentoOrdemVendaBL : PlataformaBaseBL<MovimentoOrdemVenda>
    {
        protected ProdutoBL ProdutoBL;
        protected MovimentoBL MovimentoBL;

        public MovimentoOrdemVendaBL(AppDataContextBase context, ProdutoBL produtoBL, MovimentoBL movimentoBL)
            : base(context)
        {
            ProdutoBL = produtoBL;
            MovimentoBL = movimentoBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void Insert(MovimentoOrdemVenda entity)
        {
            var produto = ProdutoBL.All.FirstOrDefault(x => x.Id == entity.ProdutoId);

            if (produto == null)
            {
                return;
            }
            else
            {
                var diferenca = produto.SaldoProduto - entity.QuantidadeBaixa;
                //se vai ficar negativo, dar entrada automática da diferenca antes
                if (diferenca < 0)
                {
                    Movimento movimentoEntrada = new Movimento()
                    {
                        QuantidadeMovimento = -diferenca,
                        ProdutoId = entity.ProdutoId,
                        Observacao = @"Observação gerada pela entrada do estoque automática, evitando estoque negativo, referente ao pedido de venda nº "+ entity.PedidoNumero.ToString() +", aplicativo Fly01 Faturamento",
                        UsuarioInclusao = entity.UsuarioInclusao,
                        PlataformaId = entity.PlataformaId
                    };
                    MovimentoBL.Movimenta(movimentoEntrada);
                }

                Movimento movimentoBaixa = new Movimento()
                {
                    QuantidadeMovimento = -entity.QuantidadeBaixa,
                    ProdutoId = entity.ProdutoId,
                    Observacao = @"Observação gerada pela saída do estoque, referente ao pedido de venda nº " + entity.PedidoNumero.ToString() + ", aplicativo Fly01 Faturamento",
                    UsuarioInclusao = entity.UsuarioInclusao,
                    PlataformaId = entity.PlataformaId
                };
                MovimentoBL.Movimenta(movimentoBaixa);
            }
        }

        public override void Update(MovimentoOrdemVenda entity)
        {
            throw new BusinessException("Não é possível alterar um movimento de baixa");
        }

        public override void Delete(MovimentoOrdemVenda entityToDelete)
        {
            throw new BusinessException("Não é possível excluir um movimento de baixa");
        }

        public static Error ProdutoNaoInformado = new Error("O produto não foi informado ou o Id é inválido.", "produtoId");
    }
}