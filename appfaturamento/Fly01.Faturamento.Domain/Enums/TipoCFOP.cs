using Fly01.Core.Entities.Attribute;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoCFOP
    {
        [Subtitle("Entrada", "Entrada")]
        Entrada = 1,
        [Subtitle("Saida", "Saída")]
        Saida = 2,
    }
}
