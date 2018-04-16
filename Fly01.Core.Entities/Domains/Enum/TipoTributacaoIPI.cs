using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoTributacaoIPI
    {
        [Subtitle("EntradaComRecuperacaoDeCredito", "00 - Entrada com Recuperação de Crédito")]
        EntradaComRecuperacaoDeCredito = 0,

        [Subtitle("EntradaTributavelComAliquotaZero", "01 - Entrada Tributada com Alíquota Zero")]
        EntradaTributavelComAliquotaZero = 1,

        [Subtitle("EntradaIsenta", "02 - Entrada Isenta")]
        EntradaIsenta = 2,

        [Subtitle("EntradaNaoTributada", "03 - Entrada Não Tributada")]
        EntradaNaoTributada = 3,

        [Subtitle("EntradaImune", "04 - Entrada Imune")]
        EntradaImune = 4,

        [Subtitle("EntradaComSuspensao", "05 - Entrada Com Suspensão")]
        EntradaComSuspensao = 5,

        [Subtitle("OutrasEntradas", "49 - Outras Entradas")]
        OutrasEntradas = 49,

        [Subtitle("SaidaTributada", "50 - Saída Tributada")]
        SaidaTributada = 50,

        [Subtitle("SaidaTributadaComAliquotaZero", "51 - Saída Tributável com Alíquota Zero")]
        SaidaTributadaComAliquotaZero = 51,

        [Subtitle("SaidaIsenta", "52 - Saída Isenta")]
        SaidaIsenta = 52,

        [Subtitle("SaidaNaoTributada", "53 - Saída Não Tributada")]
        SaidaNaoTributada = 53,

        [Subtitle("SaidaImune", "54 - Saída Imune")]
        SaidaImune = 54,

        [Subtitle("SaidaComSuspensao", "55 - Saída com Suspensão")]
        SaidaComSuspensao = 55,

        [Subtitle("OutrasSaidas", "99 - Outras Saídas")]
        OutrasSaidas = 99
    }
}