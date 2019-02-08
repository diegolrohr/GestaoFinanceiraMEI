using Fly01.Core;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot(ElementName = "dup")]
    public class Duplicata
    {
        /// <summary>
        /// informar o número da duplicata
        /// </summary>
        /// 
        [JsonProperty("nDup")]
        [XmlElement(ElementName = "nDup")]
        public string Numero { get; set; }

        /// <summary>
        /// informar a data de vencimento da duplicata como DateTime
        /// </summary>
        [XmlIgnore]
        public DateTime Vencimento { get; set; }

        /// <summary>
        /// informar a data de vencimento da duplicata como string
        /// </summary>
        /// 
        [JsonProperty("dVenc")]
        [XmlElement(ElementName = "dVenc")]
        public string VencimentoString
        {
            get { return this.Vencimento.ToString("yyyy-MM-dd"); }
            set { this.Vencimento = value.ToDateTime(Extensions.DateFormat.YYYY_MM_DD); }
        }

        /// <summary>
        /// informar o valor da duplicata
        /// </summary>
        [XmlIgnore]
        public double ValorDuplicata { get; set; }

        [JsonProperty("vDup")]
        [XmlElement(ElementName = "vDup")]
        public string ValorDuplicataString
        {
            get { return ValorDuplicata.ToString("0.00").Replace(",", "."); }
            set { ValorDuplicata = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
    }
}