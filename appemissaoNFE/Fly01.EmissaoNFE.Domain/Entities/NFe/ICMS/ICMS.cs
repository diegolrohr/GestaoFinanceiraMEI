using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    public abstract class ICMS
    {
        public ICMS()
        {

        }
        
        public ICMS(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao)
        {
            OrigemMercadoria = origemMercadoria;
            CodigoSituacaoOperacao = codigoSituacaoOperacao;
        }

        [XmlElement(ElementName = "orig")]
        public OrigemMercadoria OrigemMercadoria { get; set; }

        [XmlElement(ElementName = "CSOSN")]
        public TipoTributacaoICMS CodigoSituacaoOperacao { get; set; }
    }
}