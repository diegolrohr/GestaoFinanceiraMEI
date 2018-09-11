using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
{
    public class Servico
    {
        [XmlElement(ElementName = "codigo")]
        public int Codigo { get; set; }

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

        [XmlElement(ElementName = "quant")]
        public double Quantidade { get; set; }

        [XmlElement(ElementName = "valunit")]
        public double ValorUnitario { get; set; }

        [XmlElement(ElementName = "valtotal")]
        public double ValorTotal { get; set; }

        [XmlElement(ElementName = "basecalc")]
        public double BaseCalculo { get; set; }

        [XmlElement(ElementName = "issretido")]
        public ISSRetidoNFs ISSRetido { get; set; }

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

        [XmlElement(ElementName = "valiss")]
        public double ValorISS { get; set; }

        [XmlElement(ElementName = "valissret")]
        public double ValorISSRetido { get; set; }

        [XmlElement(ElementName = "outrasret")]
        public double OutrasRetencoes { get; set; }

        [XmlElement(ElementName = "valliq")]
        public double ValorLiquido { get; set; }

        [XmlElement(ElementName = "desccond")]
        public double DescontoCondicional { get; set; }

        [XmlElement(ElementName = "descinc")]
        public double DescontoIncondicional { get; set; }

        [XmlElement(ElementName = "cst")]
        public double CST { get; set; }

        [XmlElement(ElementName = "valrepasse")]
        public double ValorRepasse { get; set; }
    }
}
