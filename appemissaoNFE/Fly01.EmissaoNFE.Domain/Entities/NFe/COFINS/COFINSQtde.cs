using System.Xml.Serialization;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS
{
    [XmlRoot(ElementName = "COFINSQtde")]
    public class COFINSQtde : COFINS
    {
        public COFINSQtde() { }

        public COFINSQtde(string codigoSituacaoTributaria, TipoCRT tipoCRT) : base(codigoSituacaoTributaria, tipoCRT) { }

        [XmlElement(ElementName = "qBCProd")]
        public double QuantidadeVendida { get; set; }

        [XmlElement(ElementName = "vAliqProd")]
        public double ValorAliquota { get; set; }

        [XmlIgnore]
        public double ValorCOFINS { get; set; }

        [XmlElement(ElementName = "vCOFINS")]
        public string ValorCOFINSString
        {
            get { return ValorCOFINS.ToString("0.00").Replace(",", "."); }
            set { ValorCOFINS = double.Parse(value); }
        }
    }
}
