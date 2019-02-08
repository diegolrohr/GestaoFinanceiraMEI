using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum Status
    {
        [Subtitle("Aberto", "Aberto", "Aberto", "gray")]
        Aberto = 1,

        [Subtitle("Finalizado", "Finalizado", "Finalizado", "green")]
        Finalizado = 2
    }
}