using Fly01.Core.Attribute;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public enum TipoPeriodicidadeVM
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