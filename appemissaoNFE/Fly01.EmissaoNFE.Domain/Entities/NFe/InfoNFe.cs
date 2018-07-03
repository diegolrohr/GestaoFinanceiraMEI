using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class InfoNFe
    {
        /// <summary>
        /// ID da nota
        /// </summary>
        [XmlAttribute(AttributeName = "Id")]
        public string NotaId { get; set; }

        /// <summary>
        /// Versão de NFe utilizada pelo cliente
        /// </summary>
        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [XmlElement(ElementName = "ide")]
        public Identificador Identificador { get; set; }

        /// <summary>
        /// Emitente
        /// </summary>
        [XmlElement(ElementName = "emit")]
        public Emitente Emitente { get; set; }

        /// <summary>
        /// Destinatário
        /// </summary>
        [XmlElement(ElementName = "dest")]
        public Destinatario Destinatario { get; set; }
        
        /// <summary>
        /// Autorizados a baixar o XML da NFe
        /// </summary>
        [XmlElement(ElementName = "autXML")]
        public List<Autorizados> Autorizados { get; set; }
        
        /// <summary>
        /// Detalhes (Produto/Imposto) 
        /// </summary>
        [XmlElement(ElementName = "det")]
        public List<Detalhe> Detalhes { get; set; }

        /// <summary>
        /// Totais
        /// </summary>
        [XmlElement(ElementName = "total")]
        public Total Total { get; set; }

        /// <summary>
        /// Transporte
        /// </summary>
        [XmlElement(ElementName = "transp")]
        public Transporte Transporte { get; set; }

        /// <summary>
        /// Cobranca
        /// </summary>
        [XmlElement(ElementName = "cobr")]
        public Cobranca Cobranca { get; set; }

        /// <summary>
        /// Pagamento
        /// </summary>
        [XmlElement(ElementName = "pag")]
        public Pagamento Pagamento { get; set; }

        /// <summary>
        /// Informações Adicionais
        /// </summary>
        [XmlElement(ElementName = "infAdic")]
        public InformacoesAdicionais InformacoesAdicionais { get; set; }
    }
}