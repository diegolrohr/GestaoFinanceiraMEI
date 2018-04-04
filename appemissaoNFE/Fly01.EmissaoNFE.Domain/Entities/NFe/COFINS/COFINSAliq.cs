using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS
{
    [XmlRoot(ElementName = "COFINSAliq")]
    public class COFINSAliq : COFINS
    {
        public COFINSAliq() { }

        public COFINSAliq(string codigoSituacaoTributaria) : base(codigoSituacaoTributaria) { }

        [XmlElement(ElementName = "vBC")]
        public double ValorBC { get; set; }
        
        [XmlElement(ElementName = "pCOFINS")]
        public double AliquotaPercentual { get; set; }

        [XmlIgnore]
        public double ValorCOFINS { get; set; }

        [XmlElement(ElementName = "vCOFINS")]
        public string ValorCOFINSString
        {
            get { return ValorCOFINS.ToString("0.00").Replace(",", ".");  }
            set { ValorCOFINS = double.Parse(value); }
        }
    }
}
