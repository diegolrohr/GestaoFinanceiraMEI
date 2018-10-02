using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoTributacaoNFS
    {
        [XmlEnum(Name = "1")]
        [Subtitle("CreditaZonaFrancaManaus", "Credita Zona Franca de Manaus")]
        CreditaZonaFrancaManaus = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("RecolheIss", "Recolhe ISS")]
        RecolheIss = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("Isencao", "Isenção")]
        Isencao = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("Imune", "Imune")]
        Imune = 4,

        [XmlEnum(Name = "5")]
        [Subtitle("ExibilidadeSuspeJudicial", "Exibilidade Suspesa via Judicial")]
        ExibilidadeSuspeJudicial = 5,

        [XmlEnum(Name = "6")]
        [Subtitle("ExibilidadeProcessoADM", " Suspensão via Processo Administrativo")]
        ExibilidadeProcessoADM = 6
        
    }
}
