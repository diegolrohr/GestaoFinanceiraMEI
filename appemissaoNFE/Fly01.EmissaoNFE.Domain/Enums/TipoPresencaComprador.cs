using Fly01.Core.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoPresencaComprador
    {
        [XmlEnum(Name = "0")]
        [Subtitle("NaoSeAplica", "0", "Não se aplica")]
        NaoSeAplica = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("Presencial", "1", "Operação presencial")]
        Presencial = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Internet", "2", "Operação não presencial, pela Internet")]
        Internet = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("Teleatendimento", "3", "Operação não presencial, Teleatendimento")]
        Teleatendimento = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("EntregaDomicilio", "4", "NFC-e em operação com entrega a domicílio")]
        EntregaDomicilio = 4,

        [XmlEnum(Name = "9")]
        [Subtitle("Outros", "9", "Operação não presencial, outros")]
        Outros = 9
    }
}
