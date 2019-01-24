﻿using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.Totais
{
    public class ICMSTOT
    {
        public ICMSTOT()
        {
            SomatorioICMSDesonerado = 0;
        }
                
        [XmlIgnore]
        public double SomatorioBC { get; set; }

        [XmlIgnore]
        public double SomatorioICMS { get; set; }

        [XmlIgnore]
        public double SomatorioICMSDesonerado { get; set; }

        [XmlIgnore]
        public double SomatorioFCP { get; set; }

        [XmlIgnore]
        public double SomatorioFCPST { get; set; }

        [XmlIgnore]
        public double SomatorioFCPSTRetido { get; set; }

        [XmlIgnore]
        public double SomatorioBCST { get; set; }

        [XmlIgnore]
        public double SomatorioICMSST { get; set; }

        [XmlIgnore]
        public double SomatorioProdutos { get; set; }

        [XmlIgnore]
        public double ValorFrete { get; set; }

        [XmlIgnore]
        public double ValorSeguro { get; set; }

        [XmlIgnore]
        public double SomatorioDesconto { get; set; }

        [XmlIgnore]
        public double SomatorioII { get; set; }

        [XmlIgnore]
        public double SomatorioIPI { get; set; }

        [XmlIgnore]
        public double SomatorioIPIDevolucao { get; set; }

        [XmlIgnore]
        public double SomatorioPis { get; set; }

        [XmlIgnore]
        public double SomatorioCofins { get; set; }

        [XmlIgnore]
        public double SomatorioOutro { get; set; }

        [XmlIgnore]
        public double ValorTotalNF { get; set; }

        [XmlIgnore]
        public double? TotalTributosAprox { get; set; }

        public bool ShouldSerializeTotalTributosAproxString()
        {
            return TotalTributosAproxString != "0.00";
        }

        [JsonProperty("vBC")]
        [XmlElement("vBC")]
        public string SomatorioBCString
        {
            get { return SomatorioBC.ToString("0.00").Replace(",", "."); }
            set { SomatorioBC = double.Parse(value.Replace(".", ","));  }
        }

        [JsonProperty("vICMS")]
        [XmlElement("vICMS")]
        public string SomatorioICMSString
        {
            get { return SomatorioICMS.ToString("0.00").Replace(",", "."); }
            set { SomatorioICMS = double.Parse(value.Replace(".",",")); }
        }

        [JsonProperty("vICMSDeson")]
        [XmlElement("vICMSDeson")]
        public string SomatorioICMSDesoneradoString
        {
            get { return SomatorioICMSDesonerado.ToString("0.00").Replace(",", "."); }
            set { SomatorioICMSDesonerado = double.Parse(value.Replace(".", ",")); }
        }

        //OBRIGATÓRIO APENAS PARA A VERSÃO 4.0
        [JsonProperty("vFCP")]
        [XmlElement("vFCP")]
        public string SomatorioFCPString
        {
            get { return SomatorioFCP.ToString("0.00").Replace(",", "."); }
            set { SomatorioFCP = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vBCST")]
        [XmlElement("vBCST")]
        public string SomatorioBCSTString
        {
            get { return SomatorioBCST.ToString("0.00").Replace(",", "."); }
            set { SomatorioBCST = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vST")]
        [XmlElement("vST")]
        public string SomatorioICMSSTString
        {
            get { return SomatorioICMSST.ToString("0.00").Replace(",", "."); }
            set { SomatorioICMSST = double.Parse(value.Replace(".", ",")); }
        }

        //OBRIGATÓRIO APENAS PARA A VERSÃO 4.0
        [JsonProperty("vFCPST")]
        [XmlElement("vFCPST")]
        public string SomatorioFCPSTString
        {
            get { return SomatorioFCPST.ToString("0.00").Replace(",", "."); }
            set { SomatorioFCPST = double.Parse(value.Replace(".", ",")); }
        }

        //OBRIGATÓRIO APENAS PARA A VERSÃO 4.0
        [JsonProperty("vFCPSTRet")]
        [XmlElement("vFCPSTRet")]
        public string SomatorioFCPSTRetidoString
        {
            get { return SomatorioFCPSTRetido.ToString("0.00").Replace(",", "."); }
            set { SomatorioFCPSTRetido = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vProd")]
        [XmlElement("vProd")]
        public string SomatorioProdutosString
        {
            get { return SomatorioProdutos.ToString("0.00").Replace(",", "."); }
            set { SomatorioProdutos = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vFrete")]
        [XmlElement("vFrete")]
        public string ValorFreteString
        {
            get { return ValorFrete.ToString("0.00").Replace(",", "."); }
            set { ValorFrete = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vSeg")]
        [XmlElement("vSeg")]
        public string ValorSeguroString
        {
            get { return ValorSeguro.ToString("0.00").Replace(",", "."); }
            set { ValorSeguro = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vDesc")]
        [XmlElement("vDesc")]
        public string SomatorioDescontoString
        {
            get { return SomatorioDesconto.ToString("0.00").Replace(",", "."); }
            set { SomatorioDesconto = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vII")]
        [XmlElement("vII")]
        public string SomatorioIIString
        {
            get { return SomatorioII.ToString("0.00").Replace(",", "."); }
            set { SomatorioII = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vIPI")]
        [XmlElement("vIPI")]
        public string SomatorioIPIString
        {
            get { return SomatorioIPI.ToString("0.00").Replace(",", "."); }
            set { SomatorioIPI = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vIPIDevol")]
        [XmlElement("vIPIDevol")]
        public string SomatorioIPIDevolucaoString
        {
            get { return SomatorioIPIDevolucao.ToString("0.00").Replace(",", "."); }
            set { SomatorioIPIDevolucao = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vPIS")]
        [XmlElement("vPIS")]
        public string SomatorioPisString
        {
            get { return SomatorioPis.ToString("0.00").Replace(",", "."); }
            set { SomatorioPis = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vCOFINS")]
        [XmlElement("vCOFINS")]
        public string SomatorioCofinsString
        {
            get { return SomatorioCofins.ToString("0.00").Replace(",", "."); }
            set { SomatorioCofins = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vOutro")]
        [XmlElement("vOutro")]
        public string SomatorioOutroString
        {
            get { return SomatorioOutro.ToString("0.00").Replace(",", "."); }
            set { SomatorioOutro = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vNF")]
        [XmlElement("vNF")]
        public string ValorTotalNFString
        {
            get { return ValorTotalNF.ToString("0.00").Replace(",", "."); }
            set { ValorTotalNF = double.Parse(value.Replace(".", ",")); }
        }

        [JsonProperty("vTotTrib")]
        [XmlElement("vTotTrib")]
        public string TotalTributosAproxString
        {
            get { return TotalTributosAprox.HasValue ? TotalTributosAprox.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { TotalTributosAprox = double.Parse(value.Replace(".", ",")); }
        }

    }
}
