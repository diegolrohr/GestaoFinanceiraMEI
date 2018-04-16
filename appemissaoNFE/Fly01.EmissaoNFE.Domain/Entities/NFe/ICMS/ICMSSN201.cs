using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot(ElementName = "ICMSSN201")]
    public class ICMSSN201 : ICMSSN202
    {
        public ICMSSN201()
        {

        }

        public ICMSSN201(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao) : base(origemMercadoria, codigoSituacaoOperacao)
        {
        }

        [XmlElement(ElementName = "pCredSN")]
        public double AliquotaAplicavelCalculoCreditoSN { get; set; }

        [XmlElement(ElementName = "vCredICMSSN")]
        public double ValorCreditoICMS { get; set; }
    }
}
