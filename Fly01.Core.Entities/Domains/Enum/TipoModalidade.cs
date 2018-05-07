using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoModalidade
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Normal", "Emissão Normal")]
        Normal = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("FSIA", "Contingência FS-IA, com impressão do DANFE em formulário de segurança")]
        FSIA = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("SCAN", "Sistema de Contingência do Ambiente Nacional")]
        SCAN = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("DPEC", "Declaração Prévia da Emissão em Contingência")]
        DPEC = 4,

        [XmlEnum(Name = "5")]
        [Subtitle("FSDA", "Contingência FS-DA, com impressão do DANFE em formulário de segurança")]
        FSDA = 5,

        [XmlEnum(Name = "6")]
        [Subtitle("SVCAN", "SEFAZ Virtual de Contingência do AN")]
        SVCAN = 6,

        [XmlEnum(Name = "7")]
        [Subtitle("SVCRS", "SEFAZ Virtual de Contingência do RS")]
        SVCRS = 7,

        [XmlEnum(Name = "9")]
        [Subtitle("NFCe", "Contingência off-line da NFC-e")]
        NFCe = 9
    }
}