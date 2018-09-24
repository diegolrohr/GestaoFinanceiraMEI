using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    public class Atividade
    {
        [XmlElement(ElementName = "codigo")]
        public int CodigoCNAE { get; set; }

        [XmlElement(ElementName = "aliquota")]
        public int AliquotaICMS { get; set; }
    }
}
