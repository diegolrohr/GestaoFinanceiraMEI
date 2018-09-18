using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusOrdemServico
    {
        [Subtitle("EmPreenchimento", "Em Preenchimento", "EM PREENCHIMENTO", "gray")]
        EmPreenchimento = 1,

        [Subtitle("EmAberto", "Em Aberto", "EM ABERTO", "totvs-blue")]
        EmAberto = 2,

        [Subtitle("EmAndamento", "Em Andamento", "EM ANDAMENTO", "yellow")]
        EmAndamento = 3,

        [Subtitle("Concluido", "Concluído", "CONCLUÍDO", "green")]
        Concluido = 4,

        [Subtitle("Cancelado", "Cancelado", "CANCELADO", "red")]
        Cancelado = 5
    }
}
