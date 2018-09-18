using Fly01.Core.Helpers.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoTributacaoNFS
    {
        [XmlEnum(Name = "1")]
        [Subtitle("creditaZonaFrancaManaus", "Credita zona Franca de Manaus", "Credita zona Franca de Manaus")]
        CreditaZonaFrancaManaus = 1,

        [Subtitle("recolheIss", "recolhe iss", "recolhe iss")]
        [XmlEnum(Name = "2")]
        RecolheIss = 2,

        [Subtitle("isencao", "isenção", "Isençao")]
        [XmlEnum(Name = "3")]
        Isencao = 3,

        [Subtitle("Imune", "Imune", "Imune")]
        [XmlEnum(Name = "4")]
        Imune = 4,

        [Subtitle("exibilidadeSuspeJudicial", "Exibilidade Suspesa via Judicial", "Exibilidade Suspesa via Judicial")]
        [XmlEnum(Name = "5")]
        ExibilidadeSuspeJudicial = 5,

        [Subtitle("exibilidadeProcessoADM", " Suspensão via Processo Administrativo", "Suspensão via Processo Administrativo")]
        [XmlEnum(Name = "6")]
        ExibilidadeProcessoADM = 6
        
    }
}
