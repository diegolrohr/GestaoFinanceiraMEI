using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class ObservacaoDoFisco
    {
        [JsonProperty("xCampo")]
        [XmlElement(ElementName = "xCampo")]
        public string Campo { get; set; }


        [JsonProperty("xTexto")]
        [XmlElement(ElementName = "xTexto")]
        public string Texto { get; set; }
    }
}
