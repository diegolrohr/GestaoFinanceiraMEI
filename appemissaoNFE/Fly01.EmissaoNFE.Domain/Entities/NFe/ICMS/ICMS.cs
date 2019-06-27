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

        public ICMS(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao, TipoCRT tipoCRT)
        {
            OrigemMercadoria = origemMercadoria;
            CodigoSituacaoOperacao = codigoSituacaoOperacao;
            CodigoSituacaoTributaria = codigoSituacaoOperacao;
            TipoCRT = tipoCRT;
            CodigoSituacaoTributaria = codigoSituacaoOperacao;
        }

        [XmlElement("orig")]
        public OrigemMercadoria OrigemMercadoria { get; set; }

        [XmlElement("CSOSN")]
        public TipoTributacaoICMS CodigoSituacaoOperacao { get; set; }

        public bool ShouldSerializeCodigoSituacaoOperacao()
        {
            return TipoCRT != TipoCRT.RegimeNormal;
        }

        [XmlElement("CST")]
        public TipoTributacaoICMS CodigoSituacaoTributaria { get; set; }

        public bool ShouldSerializeCodigoSituacaoTributaria()
        {
            return TipoCRT == TipoCRT.RegimeNormal;
        }

        [XmlIgnore]
        public TipoCRT TipoCRT { get; set; }

    }
}