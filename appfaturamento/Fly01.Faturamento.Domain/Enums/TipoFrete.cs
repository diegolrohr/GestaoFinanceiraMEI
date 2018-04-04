using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    //códigos para xml
    public enum TipoFrete
    {
        [Subtitle("CIF", "Fornecedor paga (CIF)")]
        CIF = 0,
        [Subtitle("FOB", "Comprador paga (FOB)")]
        FOB = 1,
        [Subtitle("Terceiro", "Terceiro")]
        Terceiro = 2,
        [Subtitle("SemFrete", "Sem frete")]
        SemFrete = 9
    }
}
