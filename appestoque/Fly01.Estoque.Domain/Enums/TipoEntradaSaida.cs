using Fly01.Core.Helpers;

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