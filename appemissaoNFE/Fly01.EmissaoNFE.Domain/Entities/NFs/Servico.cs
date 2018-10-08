using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    [XmlRoot(ElementName = "servico")]
    public class Servico
    {
        [XmlElement(ElementName = "codigo")]
        public string Codigo { get; set; }

        [XmlIgnore]
        public double AliquotaIss { get; set; }

        [XmlElement(ElementName = "aliquota")]
        public string AliquotaIssString
        {
            get { return AliquotaIss.ToString("0.0000").Replace(",", "."); }
            set { AliquotaIss = double.Parse(value.Replace(".", ",")); }
        }

        [XmlElement(ElementName = "idcnae")]
        public string IdCNAE { get; set; }

        [XmlElement(ElementName = "cnae")]
        public string CNAE { get; set; }

        [XmlElement(ElementName = "codtrib")]
        public string CodigoTributario { get; set; }

        [XmlElement(ElementName = "discr")]
        public string Descricao { get; set; }

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
        public TipoSimNao ISSRetido
        {
            get
            {
                return TipoSimNao.Sim;
            }
            set { }
        }

        [XmlElement(ElementName = "valdedu")]
        public double ValorDeducoes { get; set; }

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
        /// <summary>
        /// OutrasRetenções + retenções de cada imposto(PIS, COFINS, CSLL, INSS, IR)
        /// </summary>
        [XmlElement(ElementName = "outrasret")]
        public double ValorOutrasRetencoes { get; set; }

        /// <summary>
        /// valtotal - valIssRet - outrasret(valor do título financeiro)
        /// </summary>
        [XmlIgnore]
        public double ValorLiquido
        {
            get { return ValorTotal - ValorISSRetido - ValorOutrasRetencoes; }
            set { }
        }

        [XmlElement(ElementName = "valliq")]
        public string ValorLiquidoString
        {
            get { return ValorLiquido.ToString("0.00").Replace(",", "."); }
            set { ValorLiquido = double.Parse(value.Replace(".", ",")); }
        }

        /// <summary>
        /// Se Ibge 3106200 inverter as informações das Tags
        /// </summary>
        [XmlElement(ElementName = "desccond")]
        public double DescontoCondicional
        {
            get { return CodigoIBGEPrestador == "3106200" ? DescontoIncondicional : DescontoCondicional; }
            set { }
        }

        [XmlElement(ElementName = "descinc")]
        public double DescontoIncondicional
        {
            get { return CodigoIBGEPrestador != "3106200" ? DescontoIncondicional : DescontoCondicional; }
            set { }
        }

        /// <summary>
        /// Fixo 0, Origem Nacional, conforme FIRST
        /// </summary>
        [XmlElement(ElementName = "cst")]
        public string CST
        {
            get { return "0"; }
            set { }
        }

        [XmlIgnore]
        public string CodigoNBS { get; set; }

        [XmlIgnore]
        public string CodigoIBGEPrestador { get; set; }

        /// <summary>
        /// Preencher com 0.00 para ibge específicos, senão nem mandar a tag
        /// </summary>
        [XmlElement(ElementName = "valrepasse")]
        public string ValorRepasseString
        {
            get { return 0.ToString("0.00").Replace(",", "."); }
            set { }
        }

        private bool ShouldSerializeValorRepasseString()
        {
            return ("3143302||4303103||4208450||3524006".Contains(CodigoIBGEPrestador));
        }
    }
}
