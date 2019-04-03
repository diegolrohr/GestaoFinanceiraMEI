using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    //http://www.planalto.gov.br/ccivil_03/Leis/LCP/Lcp123.htm
    public enum TipoFaixaReceitaBruta
    {
        [Subtitle("Faixa1", "1ª Faixa - até 180.000,00")]
        Faixa1 = 1,

        [Subtitle("Faixa2", "2ª Faixa - de 180.000,01 a 360.000,00")]
        Faixa2 = 2,

        [Subtitle("Faixa3", "3ª Faixa - de 360.000,01 a 720.000,00")]
        Faixa3 = 3,

        [Subtitle("Faixa4", "4ª Faixa - de 720.000,01 a 1.800.000,00")]
        Faixa4 = 4,

        [Subtitle("Faixa5", "5ª Faixa - de 1.800.000,01 a 3.600.000,00")]
        Faixa5 = 5,

        [Subtitle("Faixa6", "6ª Faixa - de 3.600.000,01 a 4.800.000,00")]
        Faixa6 = 6,
    }
}