using Fly01.Core.Entities.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoOrdemCompra
    {
        [Subtitle("Orcamento", "Orçamento", "Orçamento", "blue")]
        Orcamento = 1,
        [Subtitle("Pedido", "Pedido", "Pedido", "brown")]
        Pedido = 2
    }
}
