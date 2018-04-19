﻿using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMSSN900")]
    public class ICMSSN900 : ICMS
    {

        public ICMSSN900()
        {

        }
        public ICMSSN900(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao) : base(origemMercadoria, codigoSituacaoOperacao)
        {
        }

        [XmlElement(ElementName = "modBC", IsNullable = true)]
        public ModalidadeDeterminacaoBCICMS? ModalidadeBC { get; set; }

        public bool ShouldSerializeModalidadeBC()
        {
            return ModalidadeBC.HasValue;
        }

        [XmlElement(ElementName = "pRedBC", IsNullable = true)]
        public double? PercentualReducaoBC { get; set; }

        public bool ShouldSerializePercentualReducaoBC()
        {
            return PercentualReducaoBC.HasValue;
        }

        [XmlIgnore]
        public double? ValorBC { get; set; }

        [XmlElement("vBC")]
        public string ValorBCString
        {
            get
            {
                return ValorBC.HasValue ? ValorBC.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorBC = double.Parse(value); }
        }

        [XmlIgnore]
        public double? AliquotaICMS { get; set; }

        [XmlElement("pICMS")]
        public string AliquotaICMSString
        {
            get
            {
                return AliquotaICMS.HasValue ? AliquotaICMS.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { AliquotaICMS = double.Parse(value); }
        }

        [XmlIgnore]
        public double? ValorICMS { get; set; }

        [XmlElement("vICMS")]
        public string ValorICMSString
        {
            get
            {
                return ValorICMS.HasValue ? ValorICMS.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorICMS = double.Parse(value); }
        }

        [XmlElement(ElementName = "modBCST", IsNullable = true)]
        public ModalidadeDeterminacaoBCICMSST? ModalidadeBCST { get; set; }

        public bool ShouldSerializeModalidadeBCST()
        {
            return ModalidadeBCST.HasValue;
        }

        [XmlIgnore]
        public double? PercentualMargemValorAdicionadoST { get; set; }

        [XmlElement("pMVAST")]
        public string PercentualMargemValorAdicionadoSTString
        {
            get
            {
                return PercentualMargemValorAdicionadoST.HasValue ? PercentualMargemValorAdicionadoST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { PercentualMargemValorAdicionadoST = double.Parse(value); }
        }
        
        [XmlElement(ElementName = "pRedBCST", IsNullable = true)]
        public double? PercentualReducaoBCST { get; set; }

        public bool ShouldSerializePercentualReducaoBCST()
        {
            return PercentualReducaoBCST.HasValue;
        }

        [XmlIgnore]
        public double? ValorBCST { get; set; }

        [XmlElement("vBCST")]
        public string ValorBCSTString
        {
            get
            {
                return ValorBCST.HasValue ? ValorBCST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorBCST = double.Parse(value); }
        }
        
        [XmlIgnore]
        public double? AliquotaICMSST { get; set; }

        [XmlElement("pICMSST")]
        public string AliquotaICMSSTString
        {
            get
            {
                return AliquotaICMSST.HasValue ? AliquotaICMSST.Value.ToString().Replace(",", ".") : "0.00";
            }
            set { AliquotaICMSST = double.Parse(value); }
        }
        
        [XmlIgnore]
        public double? ValorICMSST { get; set; }

        [XmlElement("vICMSST")]
        public string ValorICMSSTString
        {
            get
            {
                return ValorICMSST.HasValue ? ValorICMSST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorICMSST = double.Parse(value); }
        }
        
        [XmlElement(ElementName = "pCredSN", IsNullable = true)]
        public double? AliquotaAplicavelCalculoCreditoSN { get; set; }

        public bool ShouldSerializeAliquotaAplicavelCalculoCreditoSN()
        {
            return AliquotaAplicavelCalculoCreditoSN.HasValue;
        }

        [XmlIgnore]
        public double? ValorCreditoICMS { get; set; }

        [XmlElement("vCredICMSSN")]
        public string ValorCreditoICMSString
        {
            get
            {
                return ValorCreditoICMS.HasValue ? ValorCreditoICMS.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorCreditoICMS = double.Parse(value); }
        }
        
        [XmlElement(ElementName = "vBCSTRet", IsNullable = true)]
        public double? ValorBCSTRetido { get; set; }
        
        public bool ShouldSerializeValorBCSTRetido()
        {
            return ValorBCSTRetido.HasValue && ValorBCSTRetido.Value > 0;
        }

        [XmlElement(ElementName = "vICMSSTRet", IsNullable = true)]
        public double? ValorICMSSTRetido { get; set; }
        
        public bool ShouldSerializeValorICMSSTRetido()
        {
            return ValorICMSSTRetido.HasValue && ValorICMSSTRetido.Value > 0;
        }

        [XmlElement(ElementName = "vBCSTDest", IsNullable = true)]
        public double? ValorBCSTDestino { get; set; }

        public bool ShouldSerializeValorBCSTDestino()
        {
            return ValorBCSTDestino.HasValue && ValorBCSTDestino.Value > 0;
        }

        [XmlElement(ElementName = "vICMSSTDest", IsNullable = true)]
        public double? ValorICMSSTUFDestino { get; set; }
                
        public bool ShouldSerializeValorICMSSTUFDestino()
        {
            return ValorICMSSTUFDestino.HasValue && ValorICMSSTUFDestino.Value > 0;
        }

        [XmlElement(ElementName = "motDesICMS", IsNullable = true)]
        public int? MotivoDesoneracaoICMS { get; set; }

        public bool ShouldSerializeMotivoDesoneracaoICMS()
        {
            return MotivoDesoneracaoICMS.HasValue;
        }

        [XmlElement(ElementName = "pBCOp", IsNullable = true)]
        public double? PercentualBCop { get; set; }

        public bool ShouldSerializePercentualBCop()
        {
            return PercentualBCop.HasValue;
        }


        [XmlElement(ElementName = "vBCFCPST", IsNullable = true)]
        public double? BaseFCPST { get; set; }
        public bool ShouldSerializeBaseFCPST()
        {
            return BaseFCPST.HasValue;
        }

        [XmlElement(ElementName = "pFCPST", IsNullable = true)]
        public double? AliquotaFCPST { get; set; }
        public bool ShouldSerializeAliquotaFCPST()
        {
            return AliquotaFCPST.HasValue;
        }

        [XmlElement(ElementName = "vFCPST", IsNullable = true)]
        public double? ValorFCPST { get; set; }
        public bool ShouldSerializeValorFCPST()
        {
            return ValorFCPST.HasValue;
        }
    }
}
