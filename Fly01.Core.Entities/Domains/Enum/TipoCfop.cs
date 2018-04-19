using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoCfop
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Entrada", "Entrada")]
        Entrada = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Saida", "Saída")]
        Saida = 2,
    }
}