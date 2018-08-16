using Fly01.Core.Helpers.Attribute;

namespace Fly01.OrdemServico.Entities.Enums
{
    public enum StatusOrdemServicoEnum
    {
        [Subtitle("EmAberto", "Em Aberto")]
        EmAberto = 1,

        [Subtitle("EmAndamento", "Em Andamento")]
        EmAndamento = 2,

        [Subtitle("Concluido", "Concluído")]
        Concluido = 3,

        [Subtitle("Cancelado", "Cancelado")]
        Cancelado = 4,
    }
}
