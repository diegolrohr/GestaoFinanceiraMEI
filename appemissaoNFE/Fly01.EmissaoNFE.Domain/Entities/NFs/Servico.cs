using Fly01.Core.Entities.Domains;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    [XmlRoot(ElementName = "servico")]
    public class Servico
    {
        [XmlElement(ElementName = "codigo")]
        public string Codigo { get; set; }

        [XmlElement(ElementName = "aliquota")]
        public double Aliquota { get; set; }

        [XmlElement(ElementName = "idcnae")]
        public int IdCNAE { get; set; }

        [XmlElement(ElementName = "cnae")]
        public int CNAE { get; set; }

        [XmlElement(ElementName = "codtrib")]
        public int CodigoTributario { get; set; }

        [XmlElement(ElementName = "discr")]
        public string Discriminacao { get; set; }

        [XmlIgnore]
        public double Quantidade { get; set; }

        [XmlElement(ElementName = "quant")]
        public string QuantidadeString
        {
            get { return Quantidade.ToString("0.00").Replace(",", "."); }
            set { Quantidade = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ValorUnitario { get; set; }

        [XmlElement(ElementName = "valunit")]
        public string ValorUnitarioString
        {
            get { return ValorUnitario.ToString("0.00").Replace(",", "."); }
            set { ValorUnitario = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ValorTotal { get; set; }

        [XmlElement(ElementName = "valtotal")]
        public string ValorTotalString
        {
            get { return ValorTotal.ToString("0.00").Replace(",", "."); }
            set { ValorTotal = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double BaseCalculo { get; set; }

        [XmlElement(ElementName = "basecalc")]
        public string BaseCalculoString
        {
            get { return BaseCalculo.ToString("0.00").Replace(",", "."); }
            set { BaseCalculo = double.Parse(value.Replace(".", ",")); }
        }

        [XmlElement(ElementName = "issretido")]
        public string ISSRetido { get; set; }

        [XmlElement(ElementName = "valdedu")]
        public double ValorDeduzido { get; set; }

        [XmlElement(ElementName = "valpis")]
        public double ValorPIS { get; set; }

        [XmlElement(ElementName = "valcof")]
        public double ValorCofins { get; set; }

        [XmlElement(ElementName = "valinss")]
        public double ValorINSS { get; set; }

        [XmlElement(ElementName = "valir")]
        public double ValorIR { get; set; }

        [XmlElement(ElementName = "valcsll")]
        public double ValorCSLL { get; set; }


        [XmlIgnore]
        public double ValorISS { get; set; }

        [XmlElement(ElementName = "valiss")]
        public string ValorISSString
        {
            get { return ValorISS.ToString("0.00").Replace(",", "."); }
            set { ValorISS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ValorISSRetido { get; set; }

        [XmlElement(ElementName = "valissret")]
        public string ValorISSRetidoString
        {
            get { return ValorISSRetido.ToString("0.00").Replace(",", "."); }
            set { ValorISSRetido = double.Parse(value.Replace(".", ",")); }
        }

        [XmlElement(ElementName = "outrasret")]
        public double OutrasRetencoes { get; set; }

        [XmlIgnore]
        public double ValorLiquido { get; set; }

        [XmlElement(ElementName = "valliq")]
        public string ValorLiquidoString
        {
            get { return ValorLiquido.ToString("0.00").Replace(",", "."); }
            set { ValorLiquido = double.Parse(value.Replace(".", ",")); }
        }

        [XmlElement(ElementName = "desccond")]
        public double DescontoCondicional { get; set; }

        [XmlElement(ElementName = "descinc")]
        public double DescontoIncondicional { get; set; }

        [XmlElement(ElementName = "cst")]
        public double CST { get; set; }


        [XmlIgnore]
        public double ValorRepasse { get; set; }

        [XmlElement(ElementName = "valrepasse")]
        public string ValorRepasseString
        {
            get { return ValorRepasse.ToString("0.00").Replace(",", "."); }
            set { ValorRepasse = double.Parse(value.Replace(".", ",")); }
        }
    }
}
