using Fly01.Core.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoFormaPagamento
    {
        [XmlEnum(Name = "0")]
        [Subtitle("AVista", "0", "Pagamento à vista")]
        AVista = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("APrazo", "1", "Pagamento à prazo")]
        APrazo = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Outros", "2", "Outros")]
        Outros = 2
    }
}
