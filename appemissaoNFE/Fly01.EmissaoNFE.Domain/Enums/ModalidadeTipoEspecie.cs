using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

public enum ModalidadeTipoEspecie
{
    [XmlEnum("1")]
    [Subtitle("Passageiro", "Passageiro")]
    Passageiro = 1,

    [XmlEnum("2")]
    [Subtitle("Carga", "Carga")]
    Carga = 2,

    [XmlEnum("3")]
    [Subtitle("Misto", "Misto")]
    Misto = 3,

    [XmlEnum("4")]
    [Subtitle("Corrida", "Corrida")]
    Corrida = 4,

    [XmlEnum("5")]
    [Subtitle("Tracao", "Tração")]
    Tracao = 5,

    [XmlEnum("6")]
    [Subtitle("Especie", "Espécie")]
    Especie = 6
}
