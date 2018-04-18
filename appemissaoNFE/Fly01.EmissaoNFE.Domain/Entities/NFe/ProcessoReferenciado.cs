using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class ProcessoReferenciado
    {
        [XmlElement(ElementName = "nProc")]
        public string IdProcessoAtoConcessorio { get; set; }

        [XmlElement(ElementName = "indProc")]
        public OrigemProcesso OrigemProcesso { get; set; }
    }
}
