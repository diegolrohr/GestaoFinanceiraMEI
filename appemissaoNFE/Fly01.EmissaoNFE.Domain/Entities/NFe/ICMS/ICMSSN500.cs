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
        
        public ICMSSN500(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao) : base(origemMercadoria, codigoSituacaoOperacao)
        {
        }
        
        [XmlElement("vBCSTRet")]
        public double ValorBCSTRetido { get; set; }

        [XmlElement("vICMSSTRet")]
        public double ValorICMSSTRetido { get; set; }

        [XmlElement("vBCFCPSTRet")]
        public double? BaseFCPSTRetido { get; set; }
        public bool ShouldSerializeBaseFCPSTRetido()
        {
            return BaseFCPSTRetido.HasValue && BaseFCPSTRetido.Value > 0;
        }

        [XmlElement("pFCPSTRet")]
        public double? AliquotaFCPSTRetido { get; set; }
        public bool ShouldSerializeAliquotaFCPSTRetido()
        {
            return AliquotaFCPSTRetido.HasValue && BaseFCPSTRetido.Value > 0;
        }

        [XmlElement("vFCPSTRet")]
        public double? ValorFCPSTRetido { get; set; }
        public bool ShouldSerializeValorFCPSTRetido()
        {
            return ValorFCPSTRetido.HasValue && BaseFCPSTRetido.Value > 0;
        }        
    }
}
