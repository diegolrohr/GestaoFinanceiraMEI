using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoCondicaoPagamento
    {
        [XmlEnum(Name = "1")]
        AVista = 1,

        [XmlEnum(Name = "3")]
        APrazo = 3
    }
}
