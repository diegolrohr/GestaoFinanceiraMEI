using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Domain.Enums
{
    public enum StatusConciliado
    {
        [Subtitle("1", "Não", "NAO", "red")]
        Nao = 1,
        [Subtitle("2", "Sim", "SIM", "green")]
        Sim = 2,
        [Subtitle("3", "Parcial","PAR","gray")]
        Parcial = 3
    }
}