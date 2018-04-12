using Fly01.Core.Entities.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoTributacaoIPI
    {
        [Subtitle("T00", "00 - Entrada com Recuperação de Crédito")]
        T00 = 1,
        [Subtitle("T01", "01 - Entrada Tributada com Alíquota Zero")]
        T01 = 2,
        [Subtitle("T02", "02 - Entrada Isenta")]
        T02 = 3,
        [Subtitle("T03", "03 - Entrada Não Tributada")]
        T03 = 4,
        [Subtitle("T04", "04 - Entrada Imune")]
        T04 = 5,
        [Subtitle("T05", "05 - Entrada com Suspensão")]
        T05 = 6,
        [Subtitle("T49", "49 - Outras Entradas")]
        T49 = 49,
        [Subtitle("T50", "50 - Saída Tributada")]
        T50 = 50,
        [Subtitle("T51", "51 - Saída Tributável com Alíquota Zero")]
        T51 = 51,
        [Subtitle("T52", "52 - Saída Isenta")]
        T52 = 52,
        [Subtitle("T53", "53 - Saída Não Tributada")]
        T53 = 53,
        [Subtitle("T54", "54 - Saída Imune")]
        T54 = 54,
        [Subtitle("T55", "55 - Saída com Suspensão")]
        T55 = 55,
        [Subtitle("T99", "99 - Outras Saídas")]
        T99 = 99
    }
}