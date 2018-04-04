using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
