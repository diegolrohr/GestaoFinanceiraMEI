using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class InfoNFe
    {
        /// <summary>
        /// ID da nota
        /// </summary>
        [JsonProperty("@id")]
        [XmlAttribute(AttributeName = "Id")]
        public string NotaId { get; set; }

        /// <summary>
        /// Versão de NFe utilizada pelo cliente
        /// </summary>
        [JsonProperty("@versao")]
        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [JsonProperty("ide")]
        [XmlElement(ElementName = "ide")]
        public Identificador Identificador { get; set; }

        /// <summary>
        /// Emitente
        /// </summary>
        [JsonProperty("emit")]
        [XmlElement(ElementName = "emit")]
        public Emitente Emitente { get; set; }

        /// <summary>
        /// Destinatário
        /// </summary>
        [JsonProperty("dest")]
        [XmlElement(ElementName = "dest")]
        public Destinatario Destinatario { get; set; }

        /// <summary>
        /// Autorizados a baixar o XML da NFe
        /// </summary>
        [JsonProperty("autXML")]
        [XmlElement(ElementName = "autXML")]
        public List<Autorizados> Autorizados { get; set; }

        /// <summary>
        /// Detalhes (Produto/Imposto) 
        /// </summary>
        [JsonProperty("det")]
        [XmlElement(ElementName = "det")]
        public List<Detalhe> Detalhes { get; set; }

        /// <summary>
        /// Totais
        /// </summary>
        [JsonProperty("total")]
        [XmlElement(ElementName = "total")]
        public Total Total { get; set; }

        /// <summary>
        /// Transporte
        /// </summary>
        [JsonProperty("transp")]
        [XmlElement(ElementName = "transp")]
        public Transporte Transporte { get; set; }

        /// <summary>
        /// Cobranca
        /// </summary>
        [JsonProperty("cobr")]
        [XmlElement(ElementName = "cobr")]
        public Cobranca Cobranca { get; set; }

        /// <summary>
        /// Pagamento
        /// </summary>
        [JsonProperty("pag")]
        [XmlElement(ElementName = "pag")]
        public Pagamento Pagamento { get; set; }

        /// <summary>
        /// Informações Adicionais
        /// </summary>
        [JsonProperty("infAdic")]
        [XmlElement(ElementName = "infAdic")]
        public InformacoesAdicionais InformacoesAdicionais { get; set; }

        [XmlElement(ElementName = "infRespTec")]
        public ResposavelTecnico ResponsavelTecnico { get; set; }
    }
}