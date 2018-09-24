using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
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

        [Subtitle("comercio_industria", "comercio/industria", "comercio/industria")]
        [XmlEnum(Name = "4")]
        Comercio_industria = 4,

        [Subtitle("legislativo_Judiciario", "Legislativo/Judiciario", "Legislativo/Judiciario")]
        [XmlEnum(Name = "5")]
        Legislativo_Judiciario = 5
        
    }
}
