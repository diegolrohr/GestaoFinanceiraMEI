using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoOperacaoSerieNotaFiscal
    {
        [Subtitle("NFe", "NFe")]
        NFe = 1,
        [Subtitle("NFSe", "NFSe")]
        NFSe = 2,
        [Subtitle("Ambas", "Ambas")]
        Ambas = 3
    }
}