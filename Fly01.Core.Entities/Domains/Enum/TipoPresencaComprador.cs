using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoPresencaComprador
    {
        [XmlEnum(Name = "0")]
        [Subtitle("NaoSeAplica", "Não se aplica")]
        NaoSeAplica = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("Presencial", "Operação presencial")]
        Presencial = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Internet", "Operação não presencial, pela Internet")]
        Internet = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("TeleAtendimento", "Operação não presencial, Teleatendimento")]
        TeleAtendimento = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("EntregaDomicilio", "NFC-e em operação com entrega a domicílio")]
        EntregaDomicilio = 4,

        //Para liberar avaliar tags xml 4.0, quando for emitida uma NFe com este indicador de presença,
        //deverá ser informada a nota referenciada a essa NFe. Caso contrário, a NFe
        //pode ser rejeitada pela regra “Rejeição 864. O objetivo é tornar possível o vínculo entre NFe de remessa
        //com as notas emitidas na entrega da mercadoria
        //[XmlEnum(Name = "5")]
        //[Subtitle("PresencialForaEstabelecimento", "Operação presencial, fora do estabelecimento;")]
        //PresencialForaEstabelecimento = 5,

        [XmlEnum(Name = "9")]
        [Subtitle("Outros", "Operação não presencial, outros")]
        Outros = 9
    }
}
