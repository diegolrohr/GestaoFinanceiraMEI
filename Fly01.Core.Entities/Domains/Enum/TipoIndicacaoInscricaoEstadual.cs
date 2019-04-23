using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoIndicacaoInscricaoEstadual
    {
        [Subtitle("ContribuinteICMS", "Contribuinte ICMS")]
        [XmlEnum(Name = "1")]
        ContribuinteICMS = 1,

        [Subtitle("ContribuinteIsento", "Contribuinte Isento")]
        [XmlEnum(Name = "2")]
        ContribuinteIsento = 2,

        [Subtitle("NaoContribuinte", "Não Contribuinte")]
        [XmlEnum(Name = "9")]
        NaoContribuinte = 9
    }
}