using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoFrete
    {
        [XmlEnum("0")]
        [Subtitle("CIF", "Contratação pelo Remetente (CIF)")]
        CIF = 0,

        [XmlEnum("1")]
        [Subtitle("FOB", "Contratação pelo Destinatário (FOB)")]
        FOB = 1,

        [XmlEnum("2")]
        [Subtitle("Terceiro", "Terceiro")]
        Terceiro = 2,

        [XmlEnum("3")]
        [Subtitle("Remetente", "Transporte próprio Remetente")]
        Remetente = 3,

        [XmlEnum("4")]
        [Subtitle("Destinatario", "Transporte próprio Destinatário")]
        Destinatario = 4,

        [XmlEnum("9")]
        [Subtitle("SemFrete", "Sem frete")]
        SemFrete = 9
    }
}