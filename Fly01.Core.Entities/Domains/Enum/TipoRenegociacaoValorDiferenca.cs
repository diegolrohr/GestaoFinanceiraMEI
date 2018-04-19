using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoRenegociacaoValorDiferenca
    {
        [Subtitle("Acrescimo", "Acréscimo")]
        Acrescimo = 1,

        [Subtitle("Desconto", "Desconto")]
        Desconto = 2,
    }
}