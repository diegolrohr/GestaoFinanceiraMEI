using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoTributacaoISS
    {
        [Subtitle("T00", "00 - Tributada integralmente")]
        T00 = 1,
        [Subtitle("T01", "01 - Tributada integralmente e com o ISSQN retido na fonte")]
        T01 = 2,
        [Subtitle("T02", "02 - Tributada integralmente e sujeita ao regime da substituição tributária")]
        T02 = 3,
        [Subtitle("T03", "03 - Tributada integralmente e com o ISSQN retido anteriormente pelo substituto tributário")]
        T03 = 4,
        [Subtitle("T04", "04 - Tributada com redução da base de cálculo")]
        T04 = 5,
        [Subtitle("T05", "05 - Tributada com redução da base de cálculo e com o ISSQN retido na fonte")]
        T05 = 6,
        [Subtitle("T06", "06 - Tributada com redução da base de cálculo e sujeita ao regime da substituição tributária")]
        T06 = 7,
        [Subtitle("T07", "07- Tributada com redução da base de cálculo e com o ISSQN retido anteriormente pelo substituto tributário")]
        T07 = 8,
        [Subtitle("T08", "08 - Isenta ou Imune")]
        T08 = 9,
        [Subtitle("T09", "09 - Não tributada")]
        T09 = 10
    }
}