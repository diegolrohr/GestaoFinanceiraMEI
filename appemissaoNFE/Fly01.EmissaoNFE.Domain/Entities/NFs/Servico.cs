﻿using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    [XmlRoot(ElementName = "servico")]
    public class Servico
    {
        [XmlIgnore]
        public bool IsServicoPrioritario { get; set; }

        [XmlElement(ElementName = "codigo")]
        public string CodigoIss { get; set; }

        [XmlIgnore]
        public double AliquotaIss { get; set; }

        [XmlElement(ElementName = "aliquota")]
        public string AliquotaIssString
        {
            get { return AliquotaIss.ToString("0.00").Replace(",", "."); }
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

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "issretido")]
        public TipoSimNao ISSRetido { get; set; }

        /// <summary>
        /// deixar zero
        /// </summary>
        [XmlElement(ElementName = "valdedu")]
        public double ValorDeducoes { get; set; }

        [XmlIgnore]
        public double ValorPIS { get; set; }

        [XmlElement(ElementName = "valpis")]
        public string ValorPISString
        {
            get { return ValorPIS.ToString("0.00").Replace(",", "."); }
            set { ValorPIS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ValorCofins { get; set; }

        [XmlElement(ElementName = "valcof")]
        public string ValorCofinsString
        {
            get { return ValorCofins.ToString("0.00").Replace(",", "."); }
            set { ValorCofins = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ValorINSS { get; set; }

        [XmlElement(ElementName = "valinss")]
        public string ValorINSSString
        {
            get { return ValorINSS.ToString("0.00").Replace(",", "."); }
            set { ValorINSS = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ValorIR { get; set; }

        [XmlElement(ElementName = "valir")]
        public string ValorIRString
        {
            get { return ValorIR.ToString("0.00").Replace(",", "."); }
            set { ValorIR = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double ValorCSLL { get; set; }

        [XmlElement(ElementName = "valcsll")]
        public string ValorCSLLString
        {
            get { return ValorCSLL.ToString("0.00").Replace(",", "."); }
            set { ValorCSLL = double.Parse(value.Replace(".", ",")); }
        }

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
        /// Se Ibge 3106200 deixar desccond = 0 e inverter com DescontoIncondicional
        /// </summary>
        [XmlIgnore]
        public double DescontoCondicional { get; set; }

        [XmlElement(ElementName = "desccond")]
        public string DescontoCondicionalString
        {
            get
            {
                return CodigoIBGEPrestador == "3106200" ? "0" : DescontoCondicional.ToString("0.00").Replace(",", ".");
            }
            set { DescontoCondicional = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double DescontoIncondicional { get; set; }
        
        [XmlElement(ElementName = "descinc")]
        public string DescontoIncondicionalString
        {
            get
            {
                return CodigoIBGEPrestador == "3106200" ?
                    DescontoCondicional.ToString("0.00").Replace(",", ".") :
                    DescontoIncondicional.ToString("0.00").Replace(",", ".");
            }
            set { DescontoIncondicional = double.Parse(value.Replace(".", ",")); }
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
