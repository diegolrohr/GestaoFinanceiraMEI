using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoOperacaoSerieNotaFiscal
    {
        [Subtitle("NFe", "NFe", "NFe", "green")]
        NFe = 1,

        [Subtitle("NFSe", "NFSe", "NFSe", "blue")]
        NFSe = 2,

        [Subtitle("Ambas", "Ambas", "Ambas", "gray")]
        Ambas = 3
    }
}