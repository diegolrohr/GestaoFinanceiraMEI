using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    //http://www.planalto.gov.br/ccivil_03/Leis/LCP/Lcp123.htm
    public enum TipoEnquadramentoEmpresa
    {
        [Subtitle("Anexo1", "1ª Faixa - até 180.000,00")]
        Anexo1 = 1,

        [Subtitle("Anexo2", "2ª Faixa - de 180.000,01 a 360.000,00")]
        Anexo2 = 2,

        [Subtitle("Anexo3", "3ª Faixa - de 360.000,01 a 720.000,00")]
        Anexo3 = 3,

        [Subtitle("Anexo4", "4ª Faixa - de 720.000,01 a 1.800.000,00")]
        Anexo4 = 4,

        [Subtitle("Anexo5", "5ª Faixa - de 1.800.000,01 a 3.600.000,00")]
        Anexo5 = 5
    }
}