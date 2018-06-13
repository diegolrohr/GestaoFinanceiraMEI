using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoFrete
    {
        [Subtitle("CIF", "Por conta Remetente (CIF)")]
        CIF = 0,

        [Subtitle("FOB", "Por conta Destinatário (FOB)")]
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