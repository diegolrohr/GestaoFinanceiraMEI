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

        [XmlElement("pFCPSTRet")]
        public double? AliquotaFCPSTRetido { get; set; }

        [XmlElement("vFCPSTRet")]
        public double? ValorFCPSTRetido { get; set; }
    }
}
