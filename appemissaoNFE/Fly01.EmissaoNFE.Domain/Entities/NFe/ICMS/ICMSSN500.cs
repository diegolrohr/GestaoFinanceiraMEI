using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot(ElementName = "ICMSSN500")]
    public class ICMSSN500 : ICMS
    {
        public ICMSSN500()
        {

        }
        
        public ICMSSN500(OrigemMercadoria origemMercadoria, CSOSN codigoSituacaoOperacao) : base(origemMercadoria, codigoSituacaoOperacao)
        {
        }
        
        [XmlElement(ElementName = "vBCSTRet")]
        public double ValorBCSTRetido { get; set; }

        [XmlElement(ElementName = "vICMSSTRet")]
        public double ValorICMSSTRetido { get; set; }
    }
}
