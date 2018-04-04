using Fly01.Core.Helpers;

namespace Fly01.Estoque.Domain.Enums
{
    public enum TipoProduto
    {
        [Subtitle("Insumo", "INSUMO", "INSUMO", "orange")]
        Insumo = 1,
        [Subtitle("ProdutoFinal", "PRODUTO FINAL", "PRODUTO FINAL", "green")]
        ProdutoFinal = 2,
        [Subtitle("Servico", "SERVIÇO", "SERVIÇO", "red")]
        Servico = 3,
        [Subtitle("Outros", "OUTROS", "OUTROS", "gray")]
        Outros = 4
    }
}