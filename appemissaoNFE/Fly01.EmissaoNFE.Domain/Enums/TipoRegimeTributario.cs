using Fly01.Core.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoRegimeTributario
    {
        [XmlEnum(Name = "1")]
        [Subtitle("MicroempresaMunicipal", "1", "Microempresa Municipal")]
        MicroempresaMunicipal = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Estimativa", "2", "Estimativa")]
        Estimativa = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("SociedadeProfissionais", "3", "Sociedade de Profissionais")]
        SociedadeProfissionais = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("Cooperativa", "4", "Cooperativa")]
        Cooperativa = 4,

        [XmlEnum(Name = "5")]
        [Subtitle("MEI", "5", "Microempresário Individual (MEI)")]
        MEI = 5,

        [XmlEnum(Name = "6")]
        [Subtitle("MEEPP", "6", "Microempresário e Empresa de Pequeno Porte (ME EPP)")]
        MEEPP = 6
    }
}
