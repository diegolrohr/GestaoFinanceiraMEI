﻿using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.PIS
{
    [XmlRoot(ElementName = "PISAliq")]
    public class PISAliq : PIS
    {
        public PISAliq() { }

        public PISAliq(string codigoSituacaoTributaria) : base(codigoSituacaoTributaria)
        {
        }

        [XmlIgnore]
        public double? ValorBCDoPIS { get; set; }

        [XmlElement(ElementName = "vBC")]
        public string ValorBCDoPISString
        {
            get { return ValorBCDoPIS.HasValue ? ValorBCDoPIS.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { ValorBCDoPIS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double? PercentualPIS { get; set; }

        [XmlElement(ElementName = "pPIS")]
        public string PercentualPISString
        {
            get { return PercentualPIS.HasValue ? PercentualPIS.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { PercentualPIS = double.Parse(value.Replace(".", ",")); }
        }


        [XmlIgnore]
        public double ValorPIS { get; set; }

        [XmlElement(ElementName = "vPIS")]
        public string ValorPISString
        {
            get { return ValorPIS.ToString("0.00").Replace(",", "."); }
            set { ValorPIS = double.Parse(value.Replace(".", ",")); }
        }
    }
}
