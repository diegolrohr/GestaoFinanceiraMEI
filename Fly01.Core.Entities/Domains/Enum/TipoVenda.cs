using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoVenda
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Normal", "Normal", "Normal")]
        Normal = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Complementar", "Complementar", "Complementar")]
        Complementar = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("Ajuste", "Ajuste", "Ajuste")]
        Ajuste = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("Devolucao", "Devolução", "Devolução")]
        Devolucao = 4
    }
}
