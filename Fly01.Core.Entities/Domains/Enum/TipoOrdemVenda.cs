using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoOrdemVenda
    {
        [Subtitle("Orcamento", "Orçamento", "Orçamento", "blue")]
        Orcamento = 1,

        [Subtitle("Pedido", "Pedido", "Pedido", "brown")]
        Pedido = 2
    }
}