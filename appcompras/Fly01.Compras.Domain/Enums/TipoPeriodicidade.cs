using Fly01.Core.Entities.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoPeriodicidade
    {
        [Subtitle("Nenhuma", "Nenhuma")]
        Nenhuma = 0,
        [Subtitle("Semanal", "Semanal")]
        Semanal = 1,
        [Subtitle("Mensal", "Mensal")]
        Mensal = 2,
        [Subtitle("Anual", "Anual")]
        Anual = 3
    }
}
