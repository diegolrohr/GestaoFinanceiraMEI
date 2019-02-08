using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("cobr")]
    public class Cobranca
    {
        /// <summary>
        /// informar o grupo de Fatura
        /// </summary>
        /// 
        [JsonProperty("fat")]
        [XmlElement(ElementName = "fat")]
        public Fatura Fatura { get; set; }

        /// <summary>
        /// informar o grupo dup de duplicatas
        /// </summary>
        /// 
        [JsonProperty("dup")]
        [XmlElement(ElementName = "dup")]
        public List<Duplicata> Duplicatas { get; set; }
    }

}
