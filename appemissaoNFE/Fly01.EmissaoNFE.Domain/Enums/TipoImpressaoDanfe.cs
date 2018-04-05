using Fly01.Core.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoImpressaoDanfe
    {
        [XmlEnum(Name = "0")]
        [Subtitle("SemDanfe", "0", "Sem geração de DANFE")]
        SemDanfe = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("Retrato", "1", "DANFE normal, Retrato")]
        Retrato = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Paisagem", "2", "DANFE normal, Paisagem")]
        Paisagem = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("Simplificado", "3", "DANFE Simplificado")]
        Simplificado = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("NFCe", "4", "DANFE NFC-e")]
        NFCe = 4,

        [XmlEnum(Name = "5")]
        [Subtitle("NFCeMensagem", "5", "DANFE NFC-e em mensagem eletrônica")]
        NFCeMensagem = 5
    }
}
