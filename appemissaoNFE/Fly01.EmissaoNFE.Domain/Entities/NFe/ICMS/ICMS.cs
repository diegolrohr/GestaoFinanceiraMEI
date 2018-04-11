using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    public abstract class ICMS
    {
        public ICMS()
        {

        }
        
        public ICMS(OrigemMercadoria origemMercadoria, CSOSN codigoSituacaoOperacao)
        {
            OrigemMercadoria = origemMercadoria;
            CodigoSituacaoOperacao = codigoSituacaoOperacao;
        }

        [XmlElement(ElementName = "orig")]
        public OrigemMercadoria OrigemMercadoria { get; set; }

        [XmlElement(ElementName = "CSOSN")]
        public CSOSN CodigoSituacaoOperacao { get; set; }
       
    }
}
