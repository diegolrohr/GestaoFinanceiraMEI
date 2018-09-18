using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
{
    public class Atividade
    {
        [XmlElement(ElementName = "codigo")]
        public int Codigo { get; set; }

        [XmlElement(ElementName = "aliquota")]
        public int Aliquota { get; set; }
    }
}
