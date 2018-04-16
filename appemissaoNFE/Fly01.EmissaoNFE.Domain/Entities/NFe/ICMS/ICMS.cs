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

        #region FCP

        /// <summary>
        /// Valor da Base de Cálculo do FCP retido por Substituição Tributária
        /// </summary>
        [XmlElement(ElementName = "vBCFCPST")]
        public double ValorBaseFCPRetidoST { get; set; }

        /// <summary>
        /// Percentual do FCP retido por Substituição Tributária
        /// </summary>
        [XmlElement(ElementName = "pFCPST")]
        public double PercentualFCPRetidoST { get; set; }

        /// <summary>
        /// Valor do FCP retido por Substituição Tributária
        /// </summary>
        [XmlElement(ElementName = "vFCPST")]
        public double ValorFCPST { get; set; }

        /// <summary>
        /// Alíquota suportada pelo Consumidor Final
        /// </summary>
        [XmlElement(ElementName = "pST")]
        public double AliquotaFCPConsumidorFinal { get; set; }

        /// <summary>
        /// Valor da Base de Cálculo do FCP retido anteriormente por ST
        /// </summary>
        [XmlElement(ElementName = "vBCFCPSTRet")]
        public double ValorBaseFCPRetidoAnteriorST { get; set; }

        /// <summary>
        /// Percentual do FCP retido anteriormente por Substituição Tributária
        /// </summary>
        [XmlElement(ElementName = "pFCPSTRet")]
        public double PercentualFCPRetidoAnteriorST { get; set; }

        /// <summary>
        /// Valor do FCP retido por Substituição Tributária
        /// </summary>
        [XmlElement(ElementName = "vFCPSTRet")]
        public double ValorFCPRetidoST { get; set; }

        #endregion FCP

    }
}
