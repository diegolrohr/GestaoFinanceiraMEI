using Fly01.Core.Attribute;

namespace Fly01.Estoque.Domain.Enums
{
    public enum InventarioStatus
    {
        [Subtitle("Aberto", "ABERTO", "ABERTO", "green")]
        Aberto = 1,
        [Subtitle("Finalizado", "FINALIZADO", "FINALIZADO", "red")]
        Finalizado = 2
    }
}