using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoRenegociacaoCalculo
    {
        [Subtitle("Valor", "Valor")]
        Valor = 1,

        [Subtitle("Percentual", "Percentual")]
        Percentual = 2,
    }
}