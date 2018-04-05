using Fly01.Estoque.Domain.Entities;
using Fly01.Estoque.Domain.Enums;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Linq;

namespace Fly01.Estoque.BL
{
    public class AjusteManualBL : PlataformaBaseBL<AjusteManual>
    {
        protected ProdutoBL ProdutoBL;
        protected MovimentoBL MovimentoBL;
        protected InventarioItemBL InventarioItemBL;

        public AjusteManualBL(AppDataContextBase context, MovimentoBL movimentoBL, ProdutoBL produtoBL, InventarioItemBL inventarioItemBL)
            : base(context)
        {
            MovimentoBL = movimentoBL;
            ProdutoBL = produtoBL;
            InventarioItemBL = inventarioItemBL;
        }

        public override void ValidaModel(AjusteManual entity)
        {
            
            if(entity.TipoEntradaSaida == TipoEntradaSaida.Saida)
            {
                var produto = ProdutoBL.All.FirstOrDefault(e => e.Id == entity.ProdutoId);
                var saldoNovo = produto.SaldoProduto - entity.Quantidade;

                Error err = new Error(string.Format(SaldoNegativo, entity.Quantidade, produto.SaldoProduto), "quantidade");

                entity.Fail(saldoNovo < 0, err);
            }

            bool inventarioAbertoComProduto = InventarioItemBL.All.Any(e => e.Inventario.Ativo && e.Inventario.InventarioStatus == InventarioStatus.Aberto && e.ProdutoId == entity.ProdutoId);
            entity.Fail(entity.Quantidade < default(double), QuantidadeNegativa);
            entity.Fail(inventarioAbertoComProduto, ProdutoUtilizadoEmInventarioAberto);
            entity.Fail(entity.Quantidade == null || entity.Quantidade.Equals(default(double)), QuantidadeVazia);
            entity.Fail(entity.Observacao?.Length > AjusteManual.ObservacaoMaxLength, ObservacaoLimite);

            base.ValidaModel(entity);
        }

        public override void Insert(AjusteManual entity)
        {
            ValidaModel(entity);

            var movimento = new Movimento()
            {
                QuantidadeMovimento = entity.Quantidade,
                Observacao = entity.Observacao,
                TipoMovimentoId = entity.TipoMovimentoId,
                ProdutoId = entity.ProdutoId,
            };

            MovimentoBL.Insert(movimento);

            entity.PlataformaId = movimento.PlataformaId; // TODO: Ver se tem como não precisar passar PlataformaId
            entity.UsuarioInclusao = movimento.UsuarioInclusao; // TODO: Ver se tem como não precisar passar UsuarioInclusao
        }

        public static Error ObservacaoLimite = new Error($"A Observação deve possuir no máximo {AjusteManual.ObservacaoMaxLength} caracteres", "descricao");
        public static Error QuantidadeVazia = new Error("A quantidade deve ser informada com um valor maior que 0 (zero)", "quantidade");
        public static Error ProdutoUtilizadoEmInventarioAberto = new Error("Impossivel realizar ajuste manual. Produto está sendo utilizado por inventário aberto.", "produtoDescricao");
        public static Error QuantidadeNegativa = new Error("A quantidade deve ser informada com um valor maior que 0 (zero)", "quantidade");
        public static string SaldoNegativo = @"Impossivel realizar a saida de {0} em produto com o saldo de {1}, pois resultará em saldo negativo";
    }
}