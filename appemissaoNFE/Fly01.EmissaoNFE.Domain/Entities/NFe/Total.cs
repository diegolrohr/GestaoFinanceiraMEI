using Fly01.EmissaoNFE.Domain.Entities.NFe.Totais;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Total
    {
        [JsonProperty("ICMSTot")]
        [XmlElement(ElementName = "ICMSTot")]
        public ICMSTOT ICMSTotal { get; set; }
    }
}
