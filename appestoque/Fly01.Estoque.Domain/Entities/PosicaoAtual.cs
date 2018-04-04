using Fly01.Core.Api.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Estoque.Domain.Entities
{
    public class PosicaoAtual : PlataformaBase
    {
        public PosicaoAtual() { }

        public PosicaoAtual(List<Produto> produtos)
        {
            Produtos = produtos;
            EstoqueTotal = Produtos.Sum(x => x.SaldoProduto);
            CustoTotal = Produtos.Sum(x => x.ValorCusto * x.SaldoProduto);
            VendaTotal = Produtos.Sum(x => x.ValorVenda * x.SaldoProduto);
        }

        public List<Produto> Produtos { get; set; }
        public double? EstoqueTotal { get; set; }
        public double? CustoTotal { get; set; }
        public double? VendaTotal { get; set; }
    }
}