using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoEntradaSaida
    {
        [Subtitle("Entrada", "Entrada", "ENTRADA", "green")]
        Entrada = 1,

        [Subtitle("Saida", "Saída", "SAÍDA", "red")]
        Saida = 2
    }
}