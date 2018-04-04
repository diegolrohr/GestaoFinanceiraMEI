using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum StatusSerieNotaFiscal
    {
        [Subtitle("Habilitada", "Habilitada", "Habilitada", "green")]
        Habilitada = 1,
        [Subtitle("Inutilizada", "Inutilizada", "Inutilizada", "grey")]
        Inutilizada = 2
    }
}