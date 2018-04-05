using Fly01.Core.Attribute;

namespace Fly01.Financeiro.Domain.Enums
{
    public enum TipoRenegociacaoCalculo
    {
        [Subtitle("Valor", "Valor")]
        Valor = 1,
        [Subtitle("Percentual", "Percentual")]
        Percentual = 2,
    }
}