using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
{
    public class InformacoesComplementares
    {
        [XmlElement(ElementName = "descricao")]
        public string Descricao { get; set; }

        [XmlElement(ElementName = "observacao")]
        public string Observacao { get; set; }
    }
}
