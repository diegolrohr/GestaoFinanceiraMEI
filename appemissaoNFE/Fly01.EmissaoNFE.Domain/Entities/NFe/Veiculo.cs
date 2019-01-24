using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("veicTransp")]
    public class Veiculo
    {
        [JsonProperty("placa")]
        [XmlElement("placa")]
        public string Placa { get; set; }

        [JsonProperty("UF")]
        [XmlElement("UF")]
        public string UF { get; set; }

        [JsonProperty("RNTC")]
        [XmlElement("RNTC")]
        public string RNTC { get; set; }
    }
}
