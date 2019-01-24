using Fly01.EmissaoNFE.Domain.Enums;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("transp")]
    public class Transporte
    {
        [JsonProperty("modFrete")]
        [XmlElement("modFrete")]
        public ModalidadeFrete ModalidadeFrete { get; set; }

        [JsonProperty("transporta")]
        [XmlElement(ElementName = "transporta", IsNullable = true)]
        public Transportadora Transportadora { get; set; }

        public bool ShouldSerializeTransportadora()
        {
            return Transportadora != null;
        }

        [JsonProperty("veicTransp")]
        [XmlElement(ElementName = "veicTransp", IsNullable = true)]
        public Veiculo Veiculo { get; set; }

        public bool ShouldSerializeVeiculo()
        {
            return Veiculo != null;
        }

        [JsonProperty("vol")]
        [XmlElement(ElementName = "vol", IsNullable = true)]
        public Volume Volume { get; set; }

        public bool ShouldSerializeVolume()
        {
            return Volume != null && ModalidadeFrete != ModalidadeFrete.SemFrete;
        }

    }
}
