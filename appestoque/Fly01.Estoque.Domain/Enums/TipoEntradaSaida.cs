using Fly01.Core.Entities.Attribute;

namespace Fly01.Estoque.Domain.Enums
{
    public enum TipoEntradaSaida
    {
        [Subtitle("Entrada", "Entrada", "ENTRADA", "green")]
        Entrada = 1,

        [Subtitle("Saida", "Saída", "SAÍDA", "red")]
        Saida = 2
    }
}