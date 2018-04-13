using Fly01.Core.Entities.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum StatusNotaFiscal
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Transmitida", "1", "Transmitida")]
        Transmitida = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Autorizada", "2", "Autorizada")]
        Autorizada = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("UsoDenegado", "3", "Uso Denegado")]
        UsoDenegado = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("NaoAutorizada", "4", "Não Autorizada")]
        NaoAutorizada = 4,

        [XmlEnum(Name = "5")]
        [Subtitle("Cancelada", "5", "Cancelada")]
        Cancelada = 5,

        [Subtitle("EmCancelamento", "Em Cancelamento", "Em Cancelamento")]
        EmCancelamento = 7,

        [XmlEnum(Name = "9")]
        [Subtitle("FalhaTransmissao", "9", "Falha na Transmissão")]
        FalhaTransmissao = 9,

        [XmlEnum(Name = "10")]
        [Subtitle("CanceladaForaPrazo", "10", "Cancelada fora do prazo")]
        CanceladaForaPrazo = 10
    }
}
