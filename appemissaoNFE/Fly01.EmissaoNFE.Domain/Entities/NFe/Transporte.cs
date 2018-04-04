using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot(ElementName = "transp")]
    public class Transporte
    {
        [XmlElement(ElementName = "modFrete")]
        public int ModalidadeFrete { get; set; }

        [XmlElement(ElementName = "transporta", IsNullable = true)]
        public Transportadora Transportadora { get; set; }

        public bool ShouldSerializeTransportadora()
        {
            return Transportadora != null;
        }
    }
}
