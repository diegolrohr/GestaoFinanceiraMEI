using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    [XmlRoot("detEvento")]
    public class CartaCorrecaoEvento// : DetalheEvento
    {
        /// <summary>
        /// Tipo evento
        /// </summary>
        [XmlElement(ElementName = "tpEvento")]
        public string TipoEvento { get; set; }

        /// <summary>
        /// Sefaz Chave Acesso Nota Fiscal
        /// </summary>
        [XmlElement(ElementName = "chNFe")]
        public string SefazChaveAcesso { get; set; }
        
        /// <summary>
        /// Correção Mensagem
        /// </summary>
        [XmlElement(ElementName = "xCorrecao")]
        public string Correcao { get; set; }
    }
}
