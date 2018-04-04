using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Domain.Enums
{
    public enum TipoCarteira
    {
        [Subtitle("Receita", "Receitas")]
        Receita = 1,
        [Subtitle("Despesa", "Despesas")]
        Despesa = 2,
    }
}