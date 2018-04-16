using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot(ElementName = "ICMSSN101")]
    public class ICMSSN101 : ICMS 
    {
        public ICMSSN101()
        {

        }
        public ICMSSN101(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao) : base(origemMercadoria,codigoSituacaoOperacao)
        {
        }

        [XmlElement(ElementName = "pCredSN")]
        public double AliquotaAplicavelCalculoCreditoSN { get; set; }

        [XmlElement(ElementName = "vCredICMSSN")]
        public double ValorCreditoICMS { get; set; }
    }
}
