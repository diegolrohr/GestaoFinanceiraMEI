using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    public class Atividade
    {
        [XmlElement(ElementName = "codigo")]
        public string CodigoCNAE { get; set; }

        [XmlIgnore]
        public double AliquotaIss { get; set; }

        [XmlElement(ElementName = "aliquota")]
        public string AliquotaIssString
        {
            get { return AliquotaIss.ToString("0.0000").Replace(",", "."); }
            set { AliquotaIss = double.Parse(value.Replace(".", ",")); }
        }
    }
}
