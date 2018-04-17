using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoCarteira
    {
        [Subtitle("Receita", "Receitas")]
        Receita = 1,
        [Subtitle("Despesa", "Despesas")]
        Despesa = 2,
    }
}
