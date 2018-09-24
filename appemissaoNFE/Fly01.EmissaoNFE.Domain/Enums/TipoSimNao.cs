using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoSimNao
    {
        [XmlEnum(Name = "1")]
        Sim = 1,

        [XmlEnum(Name = "2")]
        Nao = 2
    }
}
