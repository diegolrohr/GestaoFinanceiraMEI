using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;
using Fly01.Core;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMSSN900")]
    public class ICMSSN900 : ICMS
    {

        //http://www.flexdocs.com.br/guianfe/gerarNFe.detalhe.imp.ICMS400.html
        public ICMSSN900()
        {

        }
        public ICMSSN900(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao, TipoCRT tipoCRT) : base(origemMercadoria, codigoSituacaoOperacao, tipoCRT)
        {
        }

        [XmlElement(ElementName = "modBC", IsNullable = true)]
        public ModalidadeDeterminacaoBCICMS? ModalidadeBC { get; set; }

        public bool ShouldSerializeModalidadeBC()
        {
            return ModalidadeBC.HasValue;
        }

        [XmlIgnore]
        public double? PercentualReducaoBC { get; set; }

        [XmlElement(ElementName = "pRedBC", IsNullable = true)]
        public string PercentualReducaoBCString
        {
            get
            {
                return PercentualReducaoBC.HasValue ? PercentualReducaoBC.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { PercentualReducaoBC = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializePercentualReducaoBCString()
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
                return ValorBC.HasValue && ValorBC > 0 ? ValorBC.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorBC = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double? AliquotaICMS { get; set; }

        [XmlElement("pICMS")]
        public string AliquotaICMSString
        {
            get
            {
                return AliquotaICMS.HasValue && AliquotaICMS > 0 ? AliquotaICMS.Value.ToString("0.00").Replace(",", ".") : "0.00";
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
                return ValorICMS.HasValue && ValorICMS > 0  ? ValorICMS.Value.ToString("0.00").Replace(",", ".") : "0.00";
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
                return PercentualMargemValorAdicionadoST.HasValue && PercentualMargemValorAdicionadoST > 0 ? PercentualMargemValorAdicionadoST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { PercentualMargemValorAdicionadoST = double.Parse(value); }
        }

        [XmlIgnore]
        public double? PercentualReducaoBCST { get; set; }

        [XmlElement(ElementName = "pRedBCST", IsNullable = true)]
        public string PercentualReducaoBCSTString
        {
            get
            {
                return PercentualReducaoBCST.HasValue && PercentualReducaoBCST > 0 ? PercentualReducaoBCST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { PercentualReducaoBCST = double.Parse(value); }
        }

        public bool ShouldSerializePercentualReducaoBCSTString()
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
                return ValorBCST.HasValue && ValorBCST > 0 ? ValorBCST.Value.ToString("0.00").Replace(",", ".") : "0.00";
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
                return AliquotaICMSST.HasValue && AliquotaICMSST > 0 ? AliquotaICMSST.Value.ToString("0.00").Replace(",", ".") : "0.00";
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
                return ValorICMSST.HasValue && ValorICMSST > 0.0 ? ValorICMSST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorICMSST = double.Parse(value); }
        }

        [XmlIgnore]
        public double? BaseFCPST { get; set; }
        [XmlElement(ElementName = "vBCFCPST", IsNullable = true)]
        public string BaseFCPSTString
        {
            get
            {
                return BaseFCPST.HasValue && BaseFCPST > 0.0 ? BaseFCPST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { BaseFCPST = double.Parse(value); }
        }
        public bool ShouldSerializeBaseFCPSTString()
        {
            return (BaseFCPST.HasValue && BaseFCPST.Value > 0.0) && (AliquotaFCPST.HasValue && AliquotaFCPST.Value > 0.0) && (ValorFCPST.HasValue && ValorFCPST.Value > 0.0);
        }

        [XmlIgnore]
        public double? AliquotaFCPST { get; set; }
        [XmlElement(ElementName = "pFCPST", IsNullable = true)]
        public string AliquotaFCPSTString
        {
            get
            {
                return AliquotaFCPST.HasValue && AliquotaFCPST > 0.0 ? AliquotaFCPST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { AliquotaFCPST = double.Parse(value); }
        }
        public bool ShouldSerializeAliquotaFCPSTString()
        {
            return (BaseFCPST.HasValue && BaseFCPST.Value > 0.0) && (AliquotaFCPST.HasValue && AliquotaFCPST.Value > 0.0) && (ValorFCPST.HasValue && ValorFCPST.Value > 0.0);
        }

        [XmlIgnore]
        public double? ValorFCPST { get; set; }
        [XmlElement(ElementName = "vFCPST", IsNullable = true)]
        public string ValorFCPSTString
        {
            get
            {
                return ValorFCPST.HasValue && ValorFCPST > 0.0 ? ValorFCPST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorFCPST = double.Parse(value); }
        }
        public bool ShouldSerializeValorFCPSTString()
        {
            return (BaseFCPST.HasValue && BaseFCPST.Value > 0.0) && (AliquotaFCPST.HasValue && AliquotaFCPST.Value > 0.0) && (ValorFCPST.HasValue && ValorFCPST.Value > 0.0);
        }

        [XmlIgnore]
        public double? AliquotaAplicavelCalculoCreditoSN { get; set; }

        [XmlElement(ElementName = "pCredSN", IsNullable = true)]
        public string AliquotaAplicavelCalculoCreditoSNString
        {
            get
            {
                return AliquotaAplicavelCalculoCreditoSN.HasValue && AliquotaAplicavelCalculoCreditoSN > 0 ? AliquotaAplicavelCalculoCreditoSN.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { AliquotaAplicavelCalculoCreditoSN = double.Parse(value); }
        }

        public bool ShouldSerializeAliquotaAplicavelCalculoCreditoSNSting()
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
                return ValorCreditoICMS.HasValue && ValorCreditoICMS > 0 ? ValorCreditoICMS.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorCreditoICMS = double.Parse(value); }
        }

        [XmlIgnore]
        public double? ValorBCSTRetido { get; set; }

        [XmlElement(ElementName = "vBCSTRet", IsNullable = true)]
        public string ValorBCSTRetidoString
        {
            get
            {
                return ValorBCSTRetido.HasValue && ValorBCSTRetido > 0 ? ValorBCSTRetido.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorBCSTRetido = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializeValorBCSTRetidoString()
        {
            return ValorBCSTRetido.HasValue && ValorBCSTRetido.Value > 0;
        }

        [XmlIgnore]
        public double? ValorICMSSTRetido { get; set; }

        [XmlElement(ElementName = "vICMSSTRet", IsNullable = true)]
        public string ValorICMSSTRetidoString
        {
            get
            {
                return ValorICMSSTRetido.HasValue && ValorICMSSTRetido > 0 ? ValorICMSSTRetido.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorICMSSTRetido = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializeValorICMSSTRetidoString()
        {
            return ValorICMSSTRetido.HasValue && ValorICMSSTRetido.Value > 0;
        }

        [XmlIgnore]
        public double? ValorBCSTDestino { get; set; }

        [XmlElement(ElementName = "vBCSTDest", IsNullable = true)]
        public string ValorBCSTDestinoString
        {
            get
            {
                return ValorBCSTDestino.HasValue && ValorBCSTDestino > 0 ? ValorBCSTDestino.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorBCSTDestino = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializeValorBCSTDestinoString()
        {
            return ValorBCSTDestino.HasValue && ValorBCSTDestino.Value > 0;
        }

        [XmlIgnore]
        public double? ValorICMSSTUFDestino { get; set; }

        [XmlElement(ElementName = "vICMSSTDest", IsNullable = true)]
        public string ValorICMSSTUFDestinoString
        {
            get
            {
                return ValorICMSSTUFDestino.HasValue && ValorICMSSTUFDestino > 0 ? ValorICMSSTUFDestino.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorICMSSTUFDestino = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializeValorICMSSTUFDestinoString()
        {
            return ValorICMSSTUFDestino.HasValue && ValorICMSSTUFDestino.Value > 0;
        }

        [XmlElement(ElementName = "motDesICMS", IsNullable = true)]
        public int? MotivoDesoneracaoICMS { get; set; }

        public bool ShouldSerializeMotivoDesoneracaoICMS()
        {
            return MotivoDesoneracaoICMS.HasValue;
        }

        [XmlIgnore]
        public double? PercentualBCop { get; set; }

        [XmlElement(ElementName = "pBCOp", IsNullable = true)]
        public string PercentualBCopString
        {
            get
            {
                return PercentualBCop.HasValue && PercentualBCop > 0 ? PercentualBCop.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { PercentualBCop = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializePercentualBCopString()
        {
            return PercentualBCop.HasValue;
        }
    }
}
