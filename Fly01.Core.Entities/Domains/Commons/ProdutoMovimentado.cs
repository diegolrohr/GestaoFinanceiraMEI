namespace Fly01.Core.Entities.Domains.Commons
{
    public class ProdutoMovimentado : PlataformaBase
    {
        public string Descricao { get; set; }
        public double? SaldoProduto { get; set; }
        public int TotalMovimentos { get; set; }
    }
}