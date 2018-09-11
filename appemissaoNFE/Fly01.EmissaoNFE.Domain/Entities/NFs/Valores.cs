using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
{
    public class Valores
    {
        [XmlElement(ElementName = "iss")]
        public double ISS { get; set; }

        [XmlElement(ElementName = "issret")]
        public double ISSRetido { get; set; }

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

        [XmlElement(ElementName = "aliqiss")]
        public double AliquotasISS { get; set; }

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

        [XmlElement(ElementName = "valtotdoc")]
        public double ValorTotalDocumento { get; set; }

        [XmlElement(ElementName = "valcartri")]
        public double ValorCarTributacao { get; set; }

        [XmlElement(ElementName = "valpercartri")]
        public double ValorPercapitaTributacao { get; set; }

        [XmlElement(ElementName = "valfoncartri")]
        public double ValorFonCarTributacao { get; set; }
    }
}
