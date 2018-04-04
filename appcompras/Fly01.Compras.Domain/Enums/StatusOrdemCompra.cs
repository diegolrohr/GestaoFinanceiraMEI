using Fly01.Core.Helpers;

namespace Fly01.Compras.Domain.Enums
{
    public enum StatusOrdemCompra
    {
        [Subtitle("Aberto", "Aberto", "Aberto", "gray")]
        Aberto = 1,
        [Subtitle("Finalizado", "Finalizado", "Finalizado", "green")]
        Finalizado = 2
    }
}
