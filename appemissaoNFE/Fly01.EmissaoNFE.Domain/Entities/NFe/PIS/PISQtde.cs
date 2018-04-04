using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.PIS
{
    [XmlRoot(ElementName = "PISQtde")]
    public class PISQtde : PIS
    {
        public PISQtde() { }

        public PISQtde(string codigoSituacaoTributaria) : base(codigoSituacaoTributaria)
        {
        }

        [XmlElement(ElementName = "qBCProd")]
        public double? QuantidadeVendida { get; set; }

        [XmlElement(ElementName = "vAliqProd")]
        public double? AliquotaPISST { get; set; }

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
