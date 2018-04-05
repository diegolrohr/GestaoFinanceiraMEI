using Fly01.Core.Domain;

namespace Fly01.Estoque.Domain.Entities
{
    public class ProdutoMovimentado : PlataformaBase
    {
        public string Descricao { get; set; }
        public double? SaldoProduto { get; set; }
        public int TotalMovimentos { get; set; }
    }
}