using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot(ElementName = "fat")]
    public class Fatura
    {
        /// <summary>
        /// informar o número da fatura
        /// </summary>
        [XmlElement(ElementName = "nFat")]
        public string NumeroFatura { get; set; }

        /// <summary>
        /// informar o valor originário da fatura
        /// </summary>
        [XmlIgnore]
        public double? ValorOriginario { get; set; }

        [XmlElement(ElementName = "vOrig")]
        public string ValorOriginarioString
        {
            get { return ValorOriginario.HasValue ? ValorOriginario.Value.ToString("0.00").Replace(",", ".") : null; }
            set { ValorOriginario = double.Parse(value.Replace(".", ",")); }
        }

        public bool ShouldSerializeValorOiriginarioString()
        {
            return ValorOriginario.HasValue & ValorOriginario > 0;
        }

        /// <summary>
        /// informar o valor do desconto
        /// </summary>
        [XmlIgnore]
        public double? ValorDesconto { get; set; }

        [XmlElement(ElementName = "vDesc")]
        public string ValorDescontoString
        {
            get { return ValorDesconto.HasValue ? ValorDesconto.Value.ToString("0.00").Replace(",", ".") : null; }
            set { ValorDesconto = double.Parse(value.Replace(".", ",")); }
        }

        public bool ShouldSerializeValorDescontoString()
        {
            return ValorDesconto.HasValue;
        }

        /// <summary>
        /// informar o valor Liquido da fatura
        /// </summary>
        [XmlIgnore]
        public double? ValorLiquido { get; set; }

        [XmlElement(ElementName = "vLiq")]
        public string ValorLiquidoString
        {
            get { return ValorLiquido.HasValue ? ValorLiquido.Value.ToString("0.00").Replace(",", ".") : null; }
            set { ValorLiquido = double.Parse(value.Replace(".", ",")); }
        }

        public bool ShouldSerializeValorLiquidoString()
        {
            return ValorLiquido.HasValue & ValorLiquido > 0;
        }
    }
}