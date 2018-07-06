using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class CartaCorrecaoEvento : DetalheEvento
    {
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
