using Fly01.Core.Entities.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoVersaoNFe
    {
        [XmlEnum(Name = "3.10")]
        [Subtitle("v3", "3.10", "Versão 3.10")]
        v3 = 3,
        

        [XmlEnum(Name = "4.0")]
        [Subtitle("v4", "4.0", "Versão 4.0")]
        v4 = 4
    }
}
