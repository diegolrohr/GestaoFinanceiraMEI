using Newtonsoft.Json;
using System.Xml.Serialization;
using Fly01.Core;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Imposto
    {
        /// <summary>
        /// informar o valor total aproximado dos tributos em vendas para consumidor final.
        /// </summary>
        [XmlIgnore]
        public double TotalAprox { get; set; }

        [JsonProperty("vTotTrib")]
        [XmlElement(ElementName = "vTotTrib")]
        public string TotalAproxString
        {
            get { return TotalAprox.ToString("0.00").Replace(",", "."); }
            set { TotalAprox = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        /// <summary>
        /// informar o grupo XML ICMS com o grupo de tributos do ICMS.
        /// Obs: Informar "" se o item for sujeito ao ISSQN.
        /// </summary>
        /// 
        [JsonProperty("ICMS")]
        [XmlElement(ElementName = "ICMS")]
        public ICMS.ICMSPai ICMS { get; set; }

        /// <summary>
        /// Informar o grupo XML IPI com o grupo de tributos do IPI.
        /// Obs: 1.Se a operação não for sujeita ao IPI omita o grupo informando ""
        /// 2.Informar "" se o item for sujeito ao ISSQN.
        /// </summary>
        /// 
        [JsonProperty("IPI")]
        [XmlElement(ElementName = "IPI")]
        public IPI.IPIPai IPI { get; set; }

        /// <summary>
        /// informar o grupo XML PIS com o grupo de tributos do PIS.
        /// </summary>
        /// 
        [JsonProperty("PIS")]
        [XmlElement(ElementName = "PIS")]
        public PIS.PISPai PIS { get; set; }

        /// <summary>
        /// informar o grupo XML COFINS com o grupo de tributos do COFINS.
        /// </summary>
        /// 
        [JsonProperty("COFINS")]
        [XmlElement(ElementName = "COFINS")]
        public COFINS.COFINSPai COFINS { get; set; }

        /// <summary>
        /// informar o grupo XML PISST com o grupo de tributos do PISST.
        /// Obs: Informar "" se não existir PISST.
        /// </summary>
        /// 
        [JsonProperty("PISST")]
        [XmlElement(ElementName = "PISST")]
        public PISST PISST { get; set; }

        /// <summary>
        /// informar o grupo XML II com o grupo de tributos do II.
        /// Obs: Informar apenas quando se tratar de operação de importação, nos demais casos informe "".
        /// </summary>
        /// 
        [JsonProperty("II")]
        [XmlElement(ElementName = "II")]
        public II II { get; set; }
    }
}
