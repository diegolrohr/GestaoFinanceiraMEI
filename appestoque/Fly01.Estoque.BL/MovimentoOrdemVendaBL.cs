using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;

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
                if (entity.TipoVenda == TipoFinalidadeEmissaoNFe.Normal)
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
                        MovimentoBL.Movimenta(movimentoEntrada);
                    }
                }

                //venda normal é saída, devolução é entrada
                MovimentoEstoque movimentoBaixa = new MovimentoEstoque()
                {
                    QuantidadeMovimento = entity.TipoVenda == TipoFinalidadeEmissaoNFe.Normal ? -entity.Quantidade : entity.Quantidade,
                    ProdutoId = entity.ProdutoId,
                    Observacao = @"Observação gerada pela movimentação do estoque, referente ao pedido nº " + entity.PedidoNumero.ToString() + ", aplicativo Bemacash Faturamento",
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