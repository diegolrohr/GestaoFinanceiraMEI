using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoAtualizacaoValor
    {
        [Subtitle("Percentual", "Percentual", "Percentual", "gray")]
        Percentual = 0,

        [Subtitle("Valor", "Valor", "Valor", "green")]
        Valor = 1
    }
}