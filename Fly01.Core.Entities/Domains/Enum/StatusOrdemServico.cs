using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusOrdemServico
    {
        [Subtitle("EmAberto", "Em Aberto", "EM ABERTO", "totvs-blue")]
        EmAberto = 1,

        [Subtitle("EmAndamento", "Em Andamento", "EM ANDAMENTO", "red")]
        EmAndamento = 2,

        [Subtitle("Concluido", "Concluído", "CONCLUÍDO", "green")]
        Concluido = 3,

        [Subtitle("Cancelado", "Cancelado", "CANCELADO", "teal")]
        Cancelado = 4

    }
}
