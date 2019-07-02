using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMS60")]
    public class ICMS60 : ICMS
    {
        public ICMS60()
        {

        }
        
        public ICMS60(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao, TipoCRT tipoCRT) : base(origemMercadoria, codigoSituacaoOperacao, tipoCRT)
        {
        }

        [XmlIgnore]
        public double? ValorBCSTRetido { get; set; }

        [XmlElement("vBCSTRet")]
        public string ValorBCSTRetidoString
        {
            get
            {           
                return ValorBCSTRetido.HasValue && ValorBCSTRetido > 0 ? ValorBCSTRetido.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorBCSTRetido = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double? AliquotaConsumidorFinal { get; set; }
        //Ele é composto pela soma das tags pFCPSTRet e pICMSST
        [XmlElement(ElementName = "pST", IsNullable = true)]
        public string AliquotaConsumidorFinalString
        {
            get
            {               
                return AliquotaConsumidorFinal.HasValue && AliquotaConsumidorFinal > 0 ? AliquotaConsumidorFinal.Value.ToString("0.0000").Replace(",", ".") : "0.0000";
            }
            set { AliquotaConsumidorFinal = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double? ValorICMSSTRetido { get; set; }

        [XmlElement("vICMSSTRet")]
        public string ValorICMSSTRetidoString
        {
            get
            {                
                return ValorICMSSTRetido.HasValue && ValorICMSSTRetido > 0 ? ValorICMSSTRetido.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorICMSSTRetido = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        
     /*   [XmlIgnore]
        public double? ValorICMSSubstituto { get; set; }

        [XmlElement("vICMSSubstituto")]
        public string ValorICMSSubstitutoString
        {
            get
            {
                return ValorICMSSubstituto.HasValue && ValorICMSSubstituto > 0 ? ValorICMSSubstituto.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorICMSSubstituto = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        */
        [XmlIgnore]
        public double? BaseFCPSTRetido { get; set; }
        [XmlElement(ElementName = "vBCFCPSTRet")]
        public string BaseFCPSTRetidoString
        {
            get
            {
                return BaseFCPSTRetido.HasValue ? BaseFCPSTRetido.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { BaseFCPSTRetido = double.Parse(value); }
        }
        public bool ShouldSerializeBaseFCPSTRetidoString()
        {
            return BaseFCPSTRetido.HasValue && BaseFCPSTRetido.Value >= 0;
        }

        [XmlIgnore]
        public double? AliquotaFCPSTRetido { get; set; }
        [XmlElement(ElementName = "pFCPSTRet")]
        public string AliquotaFCPSTRetidoString
        {
            get
            {
                return AliquotaFCPSTRetido.HasValue ? AliquotaFCPSTRetido.Value.ToString("0.0000").Replace(",", ".") : "0.0000";
            }
            set { AliquotaFCPSTRetido = double.Parse(value); }
        }
        public bool ShouldSerializeAliquotaFCPSTRetidoString()
        {
            return AliquotaFCPSTRetido.HasValue && AliquotaFCPSTRetido.Value >= 0;
        }

        [XmlIgnore]
        public double? ValorFCPSTRetido { get; set; }
        [XmlElement(ElementName = "vFCPSTRet")]
        public string ValorFCPSTRetidoString
        {
            get
            {
                return ValorFCPSTRetido.HasValue ? ValorFCPSTRetido.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorFCPSTRetido = double.Parse(value); }
        }
        public bool ShouldSerializeValorFCPSTRetidoString()
        {
            return ValorFCPSTRetido.HasValue && ValorFCPSTRetido.Value >= 0;
        }
    }
}
