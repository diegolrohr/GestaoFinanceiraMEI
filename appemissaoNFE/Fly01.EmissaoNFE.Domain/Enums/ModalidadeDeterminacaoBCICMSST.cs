using Fly01.Core.Entities.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum ModalidadeDeterminacaoBCICMSST
    {
        [XmlEnum(Name = "0")]
        [Subtitle("PrecoTabeladoOuMaximoSugerido", "0", "Preço tabelado ou máximo sugerido")]
        PrecoTabeladoOuMaximoSugerido = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("ListaNegativa", "1", "Lista negativa (valor)")]
        ListaNegativa = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("ListaPositiva", "2", "Lista positiva (valor)")]
        ListaPositiva = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("ListaNeutra", "3", "Lista neutra (valor)")]
        ListaNeutra = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("MargemValorAgregado", "4", "Margem Valor Agregado (%)")]
        MargemValorAgregado = 4,

        [XmlEnum(Name = "5")]
        [Subtitle("Pauta", "5", "Pauta")]
        Pauta = 5
    }
}
