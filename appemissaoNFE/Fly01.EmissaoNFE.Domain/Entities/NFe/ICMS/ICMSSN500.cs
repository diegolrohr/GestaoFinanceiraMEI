using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMSSN500")]
    public class ICMSSN500 : ICMS
    {
        public ICMSSN500()
        {

        }
        
        public ICMSSN500(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao, TipoCRT tipoCRT) : base(origemMercadoria, codigoSituacaoOperacao, tipoCRT)
        {
        }

        [XmlIgnore]
        public double ValorBCSTRetido { get; set; }

        [XmlElement("vBCSTRet")]
        public string ValorBCSTRetidoString
        {
            get
            {
                return ValorBCSTRetido.ToString("0.00").Replace(",", ".");
            }
            set { ValorBCSTRetido = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double AliquotaConsumidorFinal { get; set; }
        //Ele é composto pela soma das tags pFCPSTRet e pICMSST
        [XmlElement(ElementName = "pST")]
        public string AliquotaConsumidorFinalString
        {
            get
            {
                return AliquotaConsumidorFinal.ToString("0.00").Replace(",", ".");
            }
            set { AliquotaConsumidorFinal = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double ValorICMSSTRetido { get; set; }

        [XmlElement("vICMSSTRet")]
        public string ValorICMSSTRetidoString
        {
            get
            {
                return ValorICMSSTRetido.ToString("0.00").Replace(",", ".");
            }
            set { ValorICMSSTRetido = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double? BaseFCPSTRetido { get; set; }
        [XmlElement(ElementName = "vBCFCPSTRet", IsNullable = true)]
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
            return BaseFCPSTRetido.HasValue && BaseFCPSTRetido.Value > 0;
        }

        [XmlIgnore]
        public double? AliquotaFCPSTRetido { get; set; }
        [XmlElement(ElementName = "pFCPSTRet", IsNullable = true)]
        public string AliquotaFCPSTRetidoString
        {
            get
            {
                return AliquotaFCPSTRetido.HasValue ? AliquotaFCPSTRetido.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { AliquotaFCPSTRetido = double.Parse(value); }
        }
        public bool ShouldSerializeAliquotaFCPSTRetidoString()
        {
            return AliquotaFCPSTRetido.HasValue && AliquotaFCPSTRetido.Value > 0;
        }

        [XmlIgnore]
        public double? ValorFCPSTRetido { get; set; }
        [XmlElement(ElementName = "vFCPSTRet", IsNullable = true)]
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
            return ValorFCPSTRetido.HasValue && ValorFCPSTRetido.Value > 0;
        }
    }
}
