using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot(ElementName = "ICMSSN202")]
    public class ICMSSN202 : ICMS
    {
        public ICMSSN202()
        {

        }

        public ICMSSN202(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao) : base(origemMercadoria, codigoSituacaoOperacao)
        {
        }


        [XmlElement(ElementName = "modBCST")]
        public ModalidadeDeterminacaoBCICMSST ModalidadeBCST { get; set; }

        [XmlElement(ElementName = "pMVAST")]
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

        [XmlElement(ElementName = "vBCST")]
        public double ValorBCST { get; set; }

        [XmlElement(ElementName = "pICMSST")]
        public double AliquotaICMSST { get; set; }

        [XmlElement(ElementName = "vICMSST")]
        public double ValorICMSST { get; set; }
    }
}
