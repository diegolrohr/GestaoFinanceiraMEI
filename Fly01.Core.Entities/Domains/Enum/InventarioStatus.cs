using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum InventarioStatus
    {
        [Subtitle("Aberto", "ABERTO", "ABERTO", "green")]
        Aberto = 1,

        [Subtitle("Finalizado", "FINALIZADO", "FINALIZADO", "red")]
        Finalizado = 2
    }
}