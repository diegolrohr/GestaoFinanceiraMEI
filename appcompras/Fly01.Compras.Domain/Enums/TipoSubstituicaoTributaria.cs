using Fly01.Core.Helpers;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoSubstituicaoTributaria
    {
        [Subtitle("Entrada", "ENTRADA", "ENTRADA", "green")]
        Entrada = 1,
        [Subtitle("Saida", "SAÍDA", "SAÍDA", "orange")]
        Saida = 2
    }
}