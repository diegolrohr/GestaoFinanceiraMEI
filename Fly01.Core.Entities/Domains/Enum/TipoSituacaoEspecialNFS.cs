using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoSituacaoEspecialNFS
    {
        [XmlEnum(Name = "0")]
        [Subtitle("Outro", "Outro", "Outro")]
        Outro = 0,

        [Subtitle("SUS", "SUS", "SUS")]
        [XmlEnum(Name = "1")]
        SUS = 1,

        [Subtitle("Executivo", "Executivo", "Executivo")]
        [XmlEnum(Name = "2")]
        Executivo = 2,

        [Subtitle("Bancos", "Bancos", "Bancos")]
        [XmlEnum(Name = "3")]
        Bancos = 3,

        [Subtitle("ComercioIndustria", "Comércio/Indústria", "Comércio/Indústria")]
        [XmlEnum(Name = "4")]
        ComercioIndustria = 4,

        [Subtitle("LegislativoJudiciario", "Legislativo/Judiciário", "Legislativo/Judiciário")]
        [XmlEnum(Name = "5")]
        LegislativoJudiciario = 5
        
    }
}
