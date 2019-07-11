using Fly01.Core;
using System.Xml.Serialization;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS
{
    [XmlRoot(ElementName = "COFINSAliq")]
    public class COFINSAliq : COFINS
    {
        public COFINSAliq() { }

        public COFINSAliq(string codigoSituacaoTributaria, TipoCRT tipoCRT) : base(codigoSituacaoTributaria, tipoCRT) { }

        [XmlIgnore]
        public double ValorBC { get; set; }

        [XmlElement(ElementName = "vBC")]
        public string ValorBCString
        {
            get { return ValorBC.ToString("0.00").Replace(",", "."); }
            set { ValorBC = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double AliquotaPercentual { get; set; }

        [XmlElement(ElementName = "pCOFINS")]
        public string AliquotaPercentualString
        {
            get { return AliquotaPercentual.ToString("0.00").Replace(",", "."); }
            set { AliquotaPercentual = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double ValorCOFINS { get; set; }

        [XmlElement(ElementName = "vCOFINS")]
        public string ValorCOFINSString
        {
            get { return ValorCOFINS.ToString("0.00").Replace(",", ".");  }
            set { ValorCOFINS = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
    }
}
