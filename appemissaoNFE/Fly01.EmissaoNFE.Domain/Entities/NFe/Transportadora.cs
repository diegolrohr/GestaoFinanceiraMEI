using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot(ElementName = "transporta")]
    public class Transportadora
    {
        [JsonProperty("CNPJ")]
        [XmlElement(ElementName = "CNPJ")]
        public string CNPJ { get; set; }

        [JsonProperty("CPF")]
        [XmlElement(ElementName = "CPF")]
        public string CPF { get; set; }

        [JsonProperty("xNome")]
        [XmlElement(ElementName = "xNome")]
        public string RazaoSocial { get; set; }

        [JsonProperty("IE")]
        [XmlElement(ElementName = "IE")]
        public string IE { get; set; }

        public bool ShouldSerializeIE()
        {
            return !string.IsNullOrEmpty(IE);
        }

        [JsonProperty("xEnder")]
        [XmlElement(ElementName = "xEnder")]
        public string Endereco { get; set; }

        [JsonProperty("xMun")]
        [XmlElement(ElementName = "xMun")]
        public string Municipio { get; set; }

        [JsonProperty("UF")]
        [XmlElement(ElementName = "UF")]
        public string UF { get; set; }
    }
}
