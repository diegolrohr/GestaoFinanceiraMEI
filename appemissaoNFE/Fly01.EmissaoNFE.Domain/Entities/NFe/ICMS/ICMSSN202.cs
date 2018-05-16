using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

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

        [XmlElement(ElementName = "pMVAST", IsNullable = true)]
        public double? PercentualMargemValorAdicionadoST { get; set; }

        public bool ShouldSerializePercentualMargemValorAdicionadoST()
        {
            return PercentualMargemValorAdicionadoST.HasValue & PercentualMargemValorAdicionadoST > 0;
        }

        [XmlElement(ElementName = "pRedBCST", IsNullable = true)]
        public double? PercentualReducaoBCST { get; set; }

        public bool ShouldSerializePercentualReducaoBCST()
        {
            return PercentualReducaoBCST.HasValue & PercentualReducaoBCST > 0;
        }

        [XmlElement("vBCST")]
        public double ValorBCST { get; set; }

        [XmlElement("pICMSST")]
        public double AliquotaICMSST { get; set; }

        [XmlElement("vICMSST")]
        public double ValorICMSST { get; set; }

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
        public bool ShouldSerializeBaseFCPSTString()
        {
            return BaseFCPST.HasValue && BaseFCPST.Value > 0;
        }

        [XmlIgnore]
        public double? AliquotaFCPST { get; set; }
        [XmlElement(ElementName = "pFCPST", IsNullable = true)]
        public string AliquotaFCPSTString
        {
            get
            {
                return AliquotaFCPST.HasValue ? AliquotaFCPST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { AliquotaFCPST = double.Parse(value); }
        }
        public bool ShouldSerializeAliquotaFCPSTString()
        {
            return AliquotaFCPST.HasValue && AliquotaFCPST.Value > 0;
        }

        [XmlIgnore]
        public double? ValorFCPST { get; set; }
        [XmlElement(ElementName = "vFCPST", IsNullable = true)]
        public string ValorFCPSTString
        {
            get
            {
                return ValorFCPST.HasValue ? ValorFCPST.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorFCPST = double.Parse(value); }
        }
        public bool ShouldSerializeValorFCPSTString()
        {
            return ValorFCPST.HasValue && ValorFCPST.Value > 0;
        }
    }
}
