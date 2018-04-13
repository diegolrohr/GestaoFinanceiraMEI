using Fly01.Core.Entities.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum ModalidadeFrete
    {
        [XmlEnum("0")]
        [Subtitle("CIF", "0", "Contratação do Frete por conta do Remetente (CIF)")]
        CIF = 0,

        [XmlEnum("1")]
        [Subtitle("FOB", "1", "Contratação do Frete por conta do Destinatário (FOB)")]
        FOB = 1,

        [XmlEnum("2")]
        [Subtitle("Terceiro", "2", "Contratação do Frete por conta de Terceiros")]
        Terceiro = 2,

        [XmlEnum("3")]
        [Subtitle("Remetente", "3", "Transporte Próprio por conta do Remetente")]
        Remetente = 3,

        [XmlEnum("4")]
        [Subtitle("Destinatario", "4", "Transporte Próprio por conta do Destinatário")]
        Destinatario = 4,

        [XmlEnum("9")]
        [Subtitle("SemFrete", "9", "Sem frete")]
        SemFrete = 9
    }
}
