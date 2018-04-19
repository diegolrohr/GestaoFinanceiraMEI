using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMSSN101")]
    public class ICMSSN101 : ICMS 
    {
        public ICMSSN101()
        {

        }
        public ICMSSN101(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao) : base(origemMercadoria,codigoSituacaoOperacao)
        {
        }

        [XmlElement("pCredSN")]
        public double AliquotaAplicavelCalculoCreditoSN { get; set; }

        [XmlElement("vCredICMSSN")]
        public double ValorCreditoICMS { get; set; }
    }
}
