using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoTributacaoNFS
    {
        [XmlEnum(Name = "6")]
        [Subtitle("DentroMunicipio", "Dentro Município")]
        DentroMunicipio = 6,

        [XmlEnum(Name = "8")]
        [Subtitle("ForaMunicipio", "Fora Município")]
        ForaMunicipio = 8,

        [XmlEnum(Name = "6")]
        [Subtitle("Isencao", "Isenção")]
        Isencao = 6,

        [XmlEnum(Name = "3")]
        [Subtitle("Imune", "Imune")]
        Imune = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("ExibilidadeSuspeJudicial", "Exibilidade Suspesa via Judicial")]
        ExibilidadeSuspeJudicial = 4,

        [XmlEnum(Name = "12")]
        [Subtitle("ExibilidadeProcessoADM", " Suspensão via Processo Administrativo")]
        ExibilidadeProcessoADM = 12
        
    }
}
