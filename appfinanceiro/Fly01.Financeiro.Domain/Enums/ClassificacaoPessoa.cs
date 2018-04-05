using Fly01.Core.Attribute;

namespace Fly01.Financeiro.Domain.Enums
{
    public enum ClassificacaoPessoa
    {
        [Subtitle("ConsumidorFinal", "Consumidor Final")]
        ConsumidorFinal = 1,
        [Subtitle("ProdutorRural", "Produtor Rural")]
        ProdutorRural = 2,
        [Subtitle("Revendedor", "Revendedor")]
        Revendedor = 3,
        [Subtitle("Solidario", "Solidário")]
        Solidario = 4,
        [Subtitle("Exportacao", "Exportação")]
        Exportacao = 5,
        [Subtitle("Importacao", "Importação")]
        Importacao = 6,
        [Subtitle("NaoSeAplica", "Não se Aplica")]
        NaoSeAplica = 7
    }
}