using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot(ElementName = "transporta")]
    public class Transportadora
    {
        [XmlElement(ElementName = "CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement(ElementName = "CPF")]
        public string CPF { get; set; }

        [XmlElement(ElementName = "xNome")]
        public string RazaoSocial { get; set; }

        [XmlElement(ElementName = "IE")]
        public string IE { get; set; }

        public bool ShouldSerializeIE()
        {
            return !string.IsNullOrEmpty(IE);
        }

        [XmlElement(ElementName = "xEnder")]
        public string Endereco { get; set; }

        [XmlElement(ElementName = "xMun")]
        public string Municipio { get; set; }

        [XmlElement(ElementName = "UF")]
        public string UF { get; set; }
    }
}
