using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum CompoemValorTotal
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Compoe", "1", "Compõe valor total")]
        Compoe = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("NaoCompoe", "2", "Não compõe valor total")]
        NaoCompoe = 2
    }
}
