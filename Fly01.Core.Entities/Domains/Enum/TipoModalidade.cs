using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoModalidade
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Normal", "1", "Emissão Normal")]
        Normal = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("FSIA", "2", "Contingência FS-IA, com impressão do DANFE em formulário de segurança")]
        FSIA = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("SCAN", "3", "Sistema de Contingência do Ambiente Nacional")]
        SCAN = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("DPEC", "4", "Declaração Prévia da Emissão em Contingência")]
        DPEC = 4,

        [XmlEnum(Name = "5")]
        [Subtitle("FSDA", "5", "Contingência FS-DA, com impressão do DANFE em formulário de segurança")]
        FSDA = 5,

        [XmlEnum(Name = "6")]
        [Subtitle("SVCAN", "6", "SEFAZ Virtual de Contingência do AN")]
        SVCAN = 6,

        [XmlEnum(Name = "7")]
        [Subtitle("SVCRS", "7", "SEFAZ Virtual de Contingência do RS")]
        SVCRS = 7,

        [XmlEnum(Name = "9")]
        [Subtitle("NFCe", "9", "Contingência off-line da NFC-e")]
        NFCe = 9
    }
}