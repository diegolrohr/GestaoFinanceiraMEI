using Fly01.Core.Helpers;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoFinalidadeEmissaoNFe
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Normal", "1", "NF-e normal")]
        Normal = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Complementar", "2", "NF-e complementar")]
        Complementar = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("Ajuste", "3", "NF-e de ajuste")]
        Ajuste = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("Devolucao", "4", "Devolução")]
        Devolucao = 4
    }
}
