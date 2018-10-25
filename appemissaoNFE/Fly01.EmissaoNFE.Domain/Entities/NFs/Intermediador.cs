using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    public class Intermediador
    {
        [XmlElement(ElementName = "razao")]
        public string RazaoSocial { get; set; }

        [XmlElement(ElementName = "cpfcnpj")]
        public string CpfCnpj { get; set; }

        [XmlElement(ElementName = "inscmun")]
        public string InscricaoMunicipal { get; set; }
    }
}
