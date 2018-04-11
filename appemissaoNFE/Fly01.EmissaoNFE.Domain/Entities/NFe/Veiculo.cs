using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("veicTransp")]
    public class Veiculo
    {
        [XmlElement("placa")]
        public string Placa { get; set; }

        [XmlElement("UF")]
        public string UF { get; set; }

        [XmlElement("RNTC")]
        public string RNTC { get; set; }
    }
}
