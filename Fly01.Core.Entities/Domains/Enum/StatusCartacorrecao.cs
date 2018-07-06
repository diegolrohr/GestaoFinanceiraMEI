using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusCartaCorrecao
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Transmitida", "Transmitida", "Transmitida", "blue")]
        Transmitida = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("RegistradoEVinculado", "Registrado e Vinculado", "Registrado e Vinculado", "green")]
        RegistradoEVinculado = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("Rejeitado", "Rejeitado", "Rejeitado", "totvs-blue")]
        Rejeitado = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("RegistroENaoVinculado", "Registrado Não Vinculado", "Registrado Não Vinculado", "red")]
        RegistradoENaoVinculado = 4,
                
        [XmlEnum(Name = "5")]
        [Subtitle("FalhaTransmissao", "Falha na Transmissão", "Falha na Transmissão", "red")]
        FalhaTransmissao = 5       
    }
}