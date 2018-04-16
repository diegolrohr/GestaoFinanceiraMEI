using Fly01.Core.Entities.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoFrete
    {
        [Subtitle("CIF", "Fornecedor paga (CIF)")]
        CIF = 0,
        [Subtitle("FOB", "Comprador paga (FOB)")]
        FOB = 1,
        [Subtitle("Terceiro", "Terceiro")]
        Terceiro = 2,
        [Subtitle("Remetente", "Transporte próprio Remetente")]
        Remetente = 3,
        [Subtitle("Destinatario", "Transporte próprio Destinatário")]
        Destinatario = 4,
        [Subtitle("SemFrete", "Sem frete")]
        SemFrete = 9
    }
}
