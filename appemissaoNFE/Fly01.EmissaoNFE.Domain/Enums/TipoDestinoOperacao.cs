using Fly01.Core.Entities.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoDestinoOperacao
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Interna", "1", "Operação interna")]
        Interna = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Interestadual", "2", "Operação interestadual")]
        Interestadual = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("Exterior", "3", "Operação com exterior")]
        Exterior = 3
    }
}
