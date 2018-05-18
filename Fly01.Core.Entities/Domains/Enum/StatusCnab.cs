using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusCnab
    {
        [Subtitle("EmAberto", "Em aberto", "ABER", "green")]
        EmAberto = 1,

        [Subtitle("Pago", "Pago", "PAGO", "red")]
        Pago = 2
    }
}