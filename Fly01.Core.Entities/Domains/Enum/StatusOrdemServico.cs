using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusOrdemServico
    {
        [Subtitle("EmAberto", "Em Aberto")]
        EmAberto = 1,

        [Subtitle("EmAndamento", "Em Andamento")]
        EmAndamento = 2,

        [Subtitle("Concluido", "Concluído")]
        Concluido = 3,

        [Subtitle("Cancelado", "Cancelado")]
        Cancelado = 4

    }
}
