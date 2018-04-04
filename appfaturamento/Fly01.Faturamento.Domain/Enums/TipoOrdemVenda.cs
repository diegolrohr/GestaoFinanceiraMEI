using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoOrdemVenda
    {
        [Subtitle("Orcamento", "Orçamento", "Orçamento", "blue")]
        Orcamento = 1,
        [Subtitle("Pedido", "Pedido", "Pedido", "brown")]
        Pedido = 2
    }
}
