using Fly01.Core.Helpers;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusCnab
    {
        [Subtitle("EmAberto", "Em aberto")]
        EmAberto = 1,

        [Subtitle("Pago", "Pago")]
        Pago = 2
    }
}