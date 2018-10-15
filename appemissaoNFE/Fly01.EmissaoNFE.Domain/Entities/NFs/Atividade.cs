using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    public class Atividade
    {
        [XmlElement(ElementName = "codigo")]
        public string CodigoCNAE { get; set; }

        [XmlElement(ElementName = "aliquota")]
        public double AliquotaIss { get; set; }
    }
}
