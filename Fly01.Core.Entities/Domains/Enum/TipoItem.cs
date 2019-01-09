using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoItem
    {
        [Subtitle("Produto", "Produto", "Produto")]
        Produto = 1,

        [Subtitle("Servico", "Serviço", "Serviço")]
        Servico = 2
    }
}