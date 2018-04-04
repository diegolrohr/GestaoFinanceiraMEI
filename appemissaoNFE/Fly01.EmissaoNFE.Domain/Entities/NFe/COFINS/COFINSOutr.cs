﻿using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS
{
    [XmlRoot(ElementName = "COFINSOutr")]
    public class COFINSOutr : COFINS
    {
        public COFINSOutr() { }

        public COFINSOutr(string codigoSituacaoTributaria) : base(codigoSituacaoTributaria) { }

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
