using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoFatorConversao
    {
        [Subtitle("Multiplicar", "Multiplicar", "Multiplicar", "gray")]
        Multiplicar = 0,

        [Subtitle("Dividir", "Dividir", "Dividir", "green")]
        Dividir = 1
    }
}