using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoVersaoNFe
    {
        [XmlEnum(Name = "4.00")]
        [Subtitle("v4", "4.00", "Versão 4.00")]
        v4 = 4
    }
}
