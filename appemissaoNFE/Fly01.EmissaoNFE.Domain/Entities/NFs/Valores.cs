using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
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

        [XmlIgnore]
        public double OutrasRetencoes { get; set; }

        [XmlElement(ElementName = "outrret")]
        public string OutrasRetencoesString
        {
            get { return OutrasRetencoes.ToString("0.00").Replace(",", "."); }
            set { OutrasRetencoes = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double PIS { get; set; }

        [XmlElement(ElementName = "pis")]
        public string PISString
        {
            get { return PIS.ToString("0.00").Replace(",", "."); }
            set { PIS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double COFINS { get; set; }

        [XmlElement(ElementName = "cofins")]
        public string COFINString
        {
            get { return COFINS.ToString("0.00").Replace(",", "."); }
            set { COFINS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double INSS { get; set; }

        [XmlElement(ElementName = "inss")]
        public string INSSString
        {
            get { return INSS.ToString("0.00").Replace(",", "."); }
            set { INSS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double IR { get; set; }

        [XmlElement(ElementName = "ir")]
        public string IRString
        {
            get { return IR.ToString("0.00").Replace(",", "."); }
            set { IR = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double CSLL { get; set; }

        [XmlElement(ElementName = "csll")]
        public string CSLLString
        {
            get { return CSLL.ToString("0.00").Replace(",", "."); }
            set { CSLL = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double AliquotasISS { get; set; }

        [XmlElement(ElementName = "aliqiss")]
        public string AliquotasISSString
        {
            get { return AliquotasISS.ToString("0.0000").Replace(",", "."); }
            set { AliquotasISS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double AliquotasPIS { get; set; }

        [XmlElement(ElementName = "aliqpis")]
        public string AliquotasPISString
        {
            get { return AliquotasPIS.ToString("0.0000").Replace(",", "."); }
            set { AliquotasPIS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double AliquotasCOFINS { get; set; }

        [XmlElement(ElementName = "aliqcof")]
        public string AliquotasCOFINSString
        {
            get { return AliquotasCOFINS.ToString("0.0000").Replace(",", "."); }
            set { AliquotasCOFINS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double AliquotasINSS { get; set; }

        [XmlElement(ElementName = "aliqinss")]
        public string AliquotasINSSString
        {
            get { return AliquotasINSS.ToString("0.0000").Replace(",", "."); }
            set { AliquotasINSS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double AliquotasIR { get; set; }

        [XmlElement(ElementName = "aliqir")]
        public string AliquotasIRString
        {
            get { return AliquotasIR.ToString("0.0000").Replace(",", "."); }
            set { AliquotasIR = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double AliquotasCSLL { get; set; }

        [XmlElement(ElementName = "aliqcsll")]
        public string AliquotasCSLLString
        {
            get { return AliquotasCSLL.ToString("0.0000").Replace(",", "."); }
            set { AliquotasCSLL = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ValorTotalDocumento { get; set; }

        [XmlElement(ElementName = "valtotdoc")]
        public string ValorTotalDocumentoString
        {
            get { return ValorTotalDocumento.ToString("0.0000").Replace(",", "."); }
            set { ValorTotalDocumento = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ValorCarTributacao { get; set; }

        [XmlElement(ElementName = "valcartri")]
        public string ValorCarTributacaoString
        {
            get { return ValorCarTributacao.ToString("0.0000").Replace(",", "."); }
            set { ValorCarTributacao = double.Parse(value.Replace(".", ",")); }
        }

        [XmlElement(ElementName = "valpercartri")]
        public string ValorPercapitaTributacao
        {
            get { return ((ValorCarTributacao / ValorTotalDocumento) * 100).ToString("0.00").Replace(",", "."); }
            set { }
        }

        [XmlElement(ElementName = "valfoncartri")]
        public string ValorFonCarTributacao
        {
            get { return "IBPT"; }
            set { }
        }
    }
}
