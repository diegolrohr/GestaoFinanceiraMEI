using Fly01.Core.Attribute;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoNotaFiscal
    {
        [Subtitle("NFe", "NF-e", "NF-e", "blue")]
        NFe = 1,
        [Subtitle("NFSe", "NFS-e", "NFS-e", "brown")]
        NFSe = 2
    }
}
