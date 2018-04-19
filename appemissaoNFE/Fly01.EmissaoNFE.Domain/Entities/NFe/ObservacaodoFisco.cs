using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class ObservacaoDoFisco
    {
        [XmlElement(ElementName = "xCampo")]
        public string Campo { get; set; }

        [XmlElement(ElementName = "xTexto")]
        public string Texto { get; set; }
    }
}
