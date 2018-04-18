using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Detalhe
    {
        /// <summary>
        /// informar o número do item do detalhe, deve ser um valor único crescente compreendido na faixa de 1 a 990.
        /// </summary>
        [XmlAttribute(AttributeName = "nItem")]
        public int NumeroItem { get; set; }

        /// <summary>
        /// informar o grupo XML prod com o detalhamento do produto/serviço do item.
        /// </summary>
        [XmlElement(ElementName = "prod")]
        public Produto Produto { get; set; }

        /// <summary>
        /// informar o grupo XML imposto com as informações dos tributos incidentes no item.
        /// </summary>
        [XmlElement(ElementName = "imposto")]
        public Imposto Imposto { get; set; }
    }
}
