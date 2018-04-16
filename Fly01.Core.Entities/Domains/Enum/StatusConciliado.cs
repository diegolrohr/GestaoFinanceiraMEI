using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
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