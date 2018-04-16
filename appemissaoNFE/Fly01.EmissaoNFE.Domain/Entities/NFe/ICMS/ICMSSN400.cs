using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot(ElementName = "ICMSSN400")]
    public class ICMSSN400 : ICMS
    {
        public ICMSSN400()
        {

        }

        public ICMSSN400(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao) : base(origemMercadoria, codigoSituacaoOperacao)
        {
        }
    }
}
