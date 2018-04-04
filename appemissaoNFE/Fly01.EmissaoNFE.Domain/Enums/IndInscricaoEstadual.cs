using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum IndInscricaoEstadual
    {
        [XmlEnum(Name = "1")]
        ContribuinteICMS = 1,

        [XmlEnum(Name = "2")]
        ContribuinteIsento = 2,

        [XmlEnum(Name = "9")]
        NaoContribuinte = 9
    }
}
