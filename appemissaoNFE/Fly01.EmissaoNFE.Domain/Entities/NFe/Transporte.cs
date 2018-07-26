using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("transp")]
    public class Transporte
    {
        [XmlElement("modFrete")]
        public ModalidadeFrete ModalidadeFrete { get; set; }

        [XmlElement("modEspecie")]
        public ModalidadeTipoEspecie ModalidadeEspecie { get; set; }

        [XmlElement(ElementName = "transporta", IsNullable = true)]
        public Transportadora Transportadora { get; set; }

        public bool ShouldSerializeTransportadora()
        {
            return Transportadora != null;
        }

        [XmlElement(ElementName = "veicTransp", IsNullable = true)]
        public Veiculo Veiculo { get; set; }

        public bool ShouldSerializeVeiculo()
        {
            return Veiculo != null;
        }

        [XmlElement(ElementName = "vol", IsNullable = true)]
        public Volume Volume { get; set; }

        public bool ShouldSerializeVolume()
        {
            return Volume != null;
        }

    }
}
