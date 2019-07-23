using Fly01.Core.Entities.Domains.Enum;
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
        }

        [XmlElement("orig")]
        public OrigemMercadoria OrigemMercadoria { get; set; }

        [XmlElement("CSOSN")]
        public TipoTributacaoICMS CodigoSituacaoOperacao { get; set; }

        public bool ShouldSerializeCodigoSituacaoOperacao()
        {
            return (int)CodigoSituacaoOperacao >= 101 && (int)CodigoSituacaoOperacao <= 900;
        }

        [XmlElement("CST")]
        public TipoTributacaoICMS CodigoSituacaoTributaria { get; set; }

        public bool ShouldSerializeCodigoSituacaoTributaria()
        {
            return (int)CodigoSituacaoOperacao >= 0 && (int)CodigoSituacaoOperacao <= 90;
        }

        [XmlIgnore]
        public TipoCRT TipoCRT { get; set; }

    }
}