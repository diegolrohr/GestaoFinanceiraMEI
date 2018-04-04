using Fly01.EmissaoNFE.Domain.Entities.NFe.Totais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Total
    {
        [XmlElement(ElementName = "ICMSTot")]
        public ICMSTOT ICMSTotal { get; set; }
    }
}
