using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot(ElementName = "ICMSSN103")]
    public class ICMSSN103 : ICMS
    {
        public ICMSSN103()
        {

        }

        public ICMSSN103(OrigemMercadoria origemMercadoria, CSOSN codigoSituacaoOperacao) : base(origemMercadoria, codigoSituacaoOperacao)
        {
        }
    }
}
