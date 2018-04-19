using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusSerieNotaFiscal
    {
        [Subtitle("Habilitada", "Habilitada", "Habilitada", "green")]
        Habilitada = 1,

        [Subtitle("Inutilizada", "Inutilizada", "Inutilizada", "grey")]
        Inutilizada = 2
    }
}