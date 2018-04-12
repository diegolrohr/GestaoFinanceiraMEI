using Fly01.Core.Entities.Attribute;

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