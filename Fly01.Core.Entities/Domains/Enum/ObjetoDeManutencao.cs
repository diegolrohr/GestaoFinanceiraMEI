using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum ObjetoDeManutencao
    {
        [Subtitle("Sim", "Sim", "Sim", "green")]
        Sim = 1,

        [Subtitle("Nao", "Não", "Não", "red")]
        Nao = 0
    }
}