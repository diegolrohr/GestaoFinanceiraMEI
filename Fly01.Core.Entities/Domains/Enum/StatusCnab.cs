using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusCnab
    {
        [Subtitle("Em aberto", "ABER")]
        EmAberto = 1,

        [Subtitle("Pago", "PAGO")]
        Pago = 2
    }
}