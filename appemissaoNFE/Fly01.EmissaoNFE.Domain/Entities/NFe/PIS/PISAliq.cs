using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.PIS
{
    [XmlRoot(ElementName = "PISAliq")]
    public class PISAliq : PIS
    {
        public PISAliq() { }

        public PISAliq(string codigoSituacaoTributaria) : base(codigoSituacaoTributaria)
        {
        }

        [XmlElement(ElementName = "vBC")]
        public double? ValorBCDoPIS { get; set; }

        [XmlElement(ElementName = "pPIS")]
        public double? PercentualPIS { get; set; }
        
        [XmlIgnore]
        public double ValorPIS { get; set; }

        [XmlElement(ElementName = "vPIS")]
        public string ValorPISString
        {
            get { return ValorPIS.ToString("0.00").Replace(",", "."); }
            set { ValorPIS = double.Parse(value); }
        }
    }
}
