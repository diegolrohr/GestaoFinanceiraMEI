using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
{
    public class Valores
    {
        [XmlIgnore]
        public double ISS { get; set; }

        [XmlElement(ElementName = "iss")]
        public string ISSString
        {
            get { return ISS.ToString("0.00").Replace(",", "."); }
            set { ISS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ISSRetido { get; set; }

        [XmlElement(ElementName = "issret")]
        public string ISSRetidoString
        {
            get { return ISSRetido.ToString("0.00").Replace(",", "."); }
            set { ISSRetido = double.Parse(value.Replace(".", ",")); }
        }

        [XmlElement(ElementName = "outrret")]
        public double OutrasRetencoes { get; set; }

        [XmlElement(ElementName = "pis")]
        public double PIS { get; set; }

        [XmlElement(ElementName = "cofins")]
        public double COFINS { get; set; }

        [XmlElement(ElementName = "inss")]
        public double INSS { get; set; }

        [XmlElement(ElementName = "ir")]
        public double IR { get; set; }

        [XmlElement(ElementName = "csll")]
        public double CSLL { get; set; }

        [XmlIgnore]
        public double AliquotasISS { get; set; }

        [XmlElement(ElementName = "aliqiss")]
        public string AliquotasISSString
        {
            get { return AliquotasISS.ToString("0.0000").Replace(",", "."); }
            set { AliquotasISS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlElement(ElementName = "aliqpis")]
        public double AliquotasPIS { get; set; }

        [XmlElement(ElementName = "aliqcof")]
        public double AliquotasCOFINS { get; set; }

        [XmlElement(ElementName = "aliqinss")]
        public double AliquotasINSS { get; set; }

        [XmlElement(ElementName = "aliqir")]
        public double AliquotasIR { get; set; }

        [XmlElement(ElementName = "aliqcsll")]
        public double AliquotasCSLL { get; set; }

        [XmlIgnore]
        public double ValorTotalDocumento { get; set; }

        [XmlElement(ElementName = "valtotdoc")]
        public string ValorTotalDocumentoString
        {
            get { return ValorTotalDocumento.ToString("0.0000").Replace(",", "."); }
            set { ValorTotalDocumento = double.Parse(value.Replace(".", ",")); }
        }

        [XmlElement(ElementName = "valcartri")]
        public double ValorCarTributacao { get; set; }

        [XmlElement(ElementName = "valpercartri")]
        public double ValorPercapitaTributacao { get; set; }

        [XmlElement(ElementName = "valfoncartri")]
        public double ValorFonCarTributacao { get; set; }
    }
}
