using Fly01.Core.Entities.Attribute;

namespace Fly01.Financeiro.Domain.Enums
{
    public enum TipoRenegociacaoValorDiferenca
    {
        [Subtitle("Acrescimo", "Acréscimo")]
        Acrescimo = 1,
        [Subtitle("Desconto", "Desconto")]
        Desconto = 2,
    }
}