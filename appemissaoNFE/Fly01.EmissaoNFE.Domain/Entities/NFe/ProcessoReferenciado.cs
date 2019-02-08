using Fly01.EmissaoNFE.Domain.Enums;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class ProcessoReferenciado
    {
        [JsonProperty("nProc")]
        [XmlElement(ElementName = "nProc")]
        public string IdProcessoAtoConcessorio { get; set; }

        [JsonProperty("indProc")]
        [XmlElement(ElementName = "indProc")]
        public OrigemProcesso OrigemProcesso { get; set; }
    }
}
