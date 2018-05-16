using System.Xml.Serialization;
namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class PISST
    {
        /// <summary>
        /// Informar o Valor da BC do PIS ST, este campo deve ser informado em caso de alíquota ad valorem.
        /// </summary>
        [XmlIgnore]
        public double? ValorBC { get; set; }
        [XmlElement(ElementName = "vBC", IsNullable = true)]
        public string SomatorioBCString
        {
            get { return ValorBC.HasValue ? ValorBC.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { ValorBC = double.Parse(value.Replace(".", ",")); }
        }
        public bool ShouldSerializeValorBCString()
        {
            return ValorBC.HasValue;
        }

        /// <summary>
        /// Informar a alíquota percentual do PIS ST, este campo deve ser informado em caso de alíquota ad valorem.
        /// </summary>
        [XmlIgnore]
        public double? AliquotaPercentual { get; set; }
        [XmlElement(ElementName = "pPIS", IsNullable = true)]
        public string AliquotaPercentualString
        {
            get { return AliquotaPercentual.HasValue ? AliquotaPercentual.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { AliquotaPercentual = double.Parse(value.Replace(".", ",")); }
        }
        public bool ShouldSerializeAliquotaPercentualString()
        {
            return AliquotaPercentual.HasValue;
        }

        /// <summary>
        /// Informar o Valor do PIS ST
        /// </summary>
        [XmlElement(ElementName = "vPIS")]
        public double ValorPISST { get; set; }

        /// <summary>
        /// Informar a quantidade vendida, este campo deve ser informado em caso de alíquota específica.
        /// </summary>
        [XmlElement(ElementName = "qBCProd", IsNullable = true)]
        public double? QuantidadeVendida { get; set; }

        public bool ShouldSerializeQuantidadeVendida()
        {
            return QuantidadeVendida.HasValue;
        }

        /// <summary>
        /// Informar a alíquota do PIS ST em reais, este campo deve ser informado em caso de alíquota específica.
        /// </summary>
        [XmlElement(ElementName = "vAliqProd", IsNullable = true)]
        public double? AliquotaPISST { get; set; }

        public bool ShouldSerializeAliquotaPISST()
        {
            return AliquotaPISST.HasValue;
        }
    }
}
