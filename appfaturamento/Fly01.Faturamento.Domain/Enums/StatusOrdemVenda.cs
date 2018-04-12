using Fly01.Core.Entities.Attribute;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum StatusOrdemVenda
    {
        [Subtitle("Aberto", "Aberto", "Aberto", "gray")]
        Aberto = 1,
        [Subtitle("Finalizado", "Finalizado", "Finalizado", "green")]
        Finalizado = 2
    }
}
