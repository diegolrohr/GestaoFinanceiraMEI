using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusNFSTSS
    {
        [XmlEnum(Name = "1")]
        [Subtitle("PendenteTransmissao", "Pendente de Transmissão", "Pendente de Transmissão", "gray")]
        PendenteTransmissao = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("AguardandoAssinatura", "Aguardando Assinatura", "Aguardando Assinatura", "blue")]
        AguardandoAssinatura = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("FalhaSchema", "Falha Schema", "Falha Schema", "red")]
        FalhaSchema = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("AguardandoRetorno", "Aguardando Retorno Web Service", "Aguardando Retorno Web Service", "blue")]
        AguardandoRetorno = 4,

        [XmlEnum(Name = "5")]
        [Subtitle("Rejeitada", "Rejeitada", "Rejeitada", "red")]
        Rejeitada = 5,

        [XmlEnum(Name = "6")]
        [Subtitle("Autorizada", "Autorizada", "Autorizada", "green")]
        Autorizada = 6,

        [XmlEnum(Name = "7")]
        [Subtitle("CanceladaInutilizada", "Cancelada ou Inutilizada", "Cancelada ou Inutilizada", "black")]
        CanceladaInutilizada = 7,
    }
}