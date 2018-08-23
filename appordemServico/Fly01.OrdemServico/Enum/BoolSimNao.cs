using Fly01.Core.Helpers.Attribute;

namespace Fly01.OrdemServico.Enum
{
    public enum BoolSimNao
    {
        [Subtitle("Sim", "Sim", "Sim", "green")]
        Sim = 1,

        [Subtitle("Nao", "Não", "Não", "red")]
        Nao = 2
    }
}