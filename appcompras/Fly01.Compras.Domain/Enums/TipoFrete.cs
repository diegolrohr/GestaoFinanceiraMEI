using Fly01.Core.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoFrete
    {
        [Subtitle("SemFrete", "Sem frete")]
        SemFrete = 1,
        [Subtitle("CIF", "Fornecedor paga (CIF)")]
        CIF = 2,
        [Subtitle("FOB", "Comprador paga (FOB)")]
        FOB = 3,
        [Subtitle("Terceiro", "Terceiro")]
        Terceiro = 4
    }
}
