using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using System.Xml.Serialization;
using Fly01.EmissaoNFE.Domain.Enums;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMS30")]
    public class ICMS30 : ICMS
    {
        public ICMS30()
        {

        }

        public ICMS30(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao, TipoCRT tipoCRT) : base(origemMercadoria, codigoSituacaoOperacao, tipoCRT)
        {
        }       

        [XmlElement(ElementName = "modBCST")]
        public ModalidadeDeterminacaoBCICMSST ModalidadeBCST { get; set; }

        [XmlIgnore]
        public double PercentualMargemValorAdicionadoST { get; set; }

        [XmlElement("pMVAST")]
        public string PercentualMargemValorAdicionadoSTString
        {
            get
            {
                return PercentualMargemValorAdicionadoST.ToString("0.0000").Replace(",", ".");
            }
            set { PercentualMargemValorAdicionadoST = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double PercentualReducaoBCST { get; set; }

        [XmlElement("pRedBCST")]
        public string PercentualReducaoBCSTString
        {
            get
            {
                return PercentualReducaoBCST.ToString("0.0000").Replace(",", ".");
            }
            set { PercentualReducaoBCST = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
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
        public string AliquotaICMSSSTtring
        {
            get
            {
                return AliquotaICMSST.ToString("0.0000").Replace(",", ".");
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

        [XmlElement(ElementName = "motDesICMS", IsNullable = true)]
        public int? MotivoDesoneracaoICMS { get; set; }

        public bool ShouldSerializeMotivoDesoneracaoICMS()
        {
            return MotivoDesoneracaoICMS.HasValue;
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
    }
}
