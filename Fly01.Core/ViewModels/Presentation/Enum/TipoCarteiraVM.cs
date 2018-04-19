using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.ViewModels.Presentation.Enum
{
    public enum TipoCarteiraVM
    {
        [Subtitle("Receita", "Receitas")]
        Receita = 1,

        [Subtitle("Despesa", "Despesas")]
        Despesa = 2,

        [Subtitle("SaidaDepreciar", "Saída a depreciar")]
        SaidaDepreciar = 3,

        [Subtitle("SaidaApropriar", "Saída a apropriar")]
        SaidaApropriar = 4,

        [Subtitle("SaidaProvisao", "Saída de provisão")]
        SaidaProvisao = 5,

        [Subtitle("SaidaNaoOperacional", "Saída não operacional")]
        SaidaNaoOperacional = 6,

        [Subtitle("EntradaNaoOperacional", "Entrada não operacional")]
        EntradaNaoOperacional = 7,

        [Subtitle("Depreciacao", "Depreciação")]
        Depreciacao = 8,

        [Subtitle("ApropriacaoCustos", "Apropriação de custos")]
        ApropriacaoCustos = 9
    }
}
