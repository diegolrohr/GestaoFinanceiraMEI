﻿using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;
using Fly01.Core;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMSSN202")]
    public class ICMSSN202 : ICMS
    {
        public ICMSSN202()
        {

        }

        public ICMSSN202(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao) : base(origemMercadoria, codigoSituacaoOperacao)
        {
        }


        [XmlElement("modBCST")]
        public ModalidadeDeterminacaoBCICMSST ModalidadeBCST { get; set; }

        [XmlIgnore]
        public double? PercentualMargemValorAdicionadoST { get; set; }

        [XmlElement(ElementName = "pMVAST", IsNullable = true)]
        public string PercentualMargemValorAdicionadoSTString
        {
            get
            {
                return PercentualMargemValorAdicionadoST.HasValue ? PercentualMargemValorAdicionadoST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { PercentualMargemValorAdicionadoST = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializePercentualMargemValorAdicionadoSTString()
        {
            return PercentualMargemValorAdicionadoST.HasValue & PercentualMargemValorAdicionadoST > 0;
        }

        [XmlIgnore]
        public double? PercentualReducaoBCST { get; set; }

        [XmlElement(ElementName = "pRedBCST", IsNullable = true)]
        public string PercentualReducaoBCSTString
        {
            get
            {
                return PercentualReducaoBCST.HasValue ? PercentualReducaoBCST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { PercentualReducaoBCST = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializePercentualReducaoBCSTString()
        {
            return PercentualReducaoBCST.HasValue & PercentualReducaoBCST > 0;
        }

        [XmlIgnore]
        public double ValorBCST { get; set; }

        [XmlElement("vBCST")]
        public string ValorBCSTString
        {
            get
            {
                return ValorBCST.ToString("0.00").Replace(",", ".");
            }
            set { ValorBCST = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double AliquotaICMSST { get; set; }
        
        [XmlElement("pICMSST")]
        public string AliquotaICMSSTString
        {
            get
            {
                return AliquotaICMSST.ToString("0.00").Replace(",", ".");
            }
            set { AliquotaICMSST = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double ValorICMSST { get; set; }

        [XmlElement("vICMSST")]
        public string ValorICMSSTString
        {
            get
            {
                return ValorICMSST.ToString("0.00").Replace(",", ".");
            }
            set { ValorICMSST = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double? BaseFCPST { get; set; }
        [XmlElement(ElementName = "vBCFCPST", IsNullable = true)]
        public string BaseFCPSTString
        {
            get
            {
                return BaseFCPST.HasValue ? BaseFCPST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { BaseFCPST = double.Parse(value); }
        }

        [XmlIgnore]
        public double? AliquotaFCPST { get; set; }
        [XmlElement(ElementName = "pFCPST", IsNullable = true)]
        public string AliquotaFCPSTString
        {
            get
            {
                return (AliquotaFCPST.HasValue && AliquotaFCPST > 0) ? AliquotaFCPST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { AliquotaFCPST = double.Parse(value); }
        }

        [XmlIgnore]
        public double? ValorFCPST { get; set; }
        [XmlElement(ElementName = "vFCPST", IsNullable = true)]
        public string ValorFCPSTString
        {
            get
            {
                return (ValorFCPST.HasValue && ValorFCPST > 0) ? ValorFCPST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorFCPST = double.Parse(value); }
        }
    }
}
