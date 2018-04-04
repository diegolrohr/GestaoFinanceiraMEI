using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.PIS
{
    [XmlRoot(ElementName = "PISOutr")]
    public class PISOutr : PIS
    {
        public PISOutr() { }

        public PISOutr(string codigoSituacaoTributaria) : base(codigoSituacaoTributaria) { }

        [XmlElement(ElementName = "vBC")]
        public double? ValorBCDoPIS { get; set; }
        public bool ShouldSerializeValorBCDoPIS()
        {
            return ValorBCDoPIS.HasValue;
        }

        [XmlElement(ElementName = "pPIS")]
        public double? PercentualPIS { get; set; }
        public bool ShouldSerializePercentualPIS()
        {
            return PercentualPIS.HasValue;
        }

        [XmlElement(ElementName = "qBCProd", IsNullable = true)]
        public double? QuantidadeVendida { get; set; }
        public bool ShouldSerializeQuantidadeVendida()
        {
            return QuantidadeVendida.HasValue;
        }

        [XmlElement(ElementName = "vAliqProd", IsNullable = true)]
        public double? AliquotaPISST { get; set; }
        public bool ShouldSerializeAliquotaPISST()
        {
            return AliquotaPISST.HasValue;
        }

        [XmlIgnore]
        public double? ValorPIS { get; set; }

        [XmlElement(ElementName = "vPIS", IsNullable = true)]
        public string ValorPISString
        {
            get { return ValorPIS.HasValue ? ValorPIS.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { ValorPIS = double.Parse(value); }
        }
        public bool ShouldSerializeValorPISString()
        {
            return ValorPIS.HasValue;
        }
    }
}
