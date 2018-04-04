using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum OrigemProcesso
    {
        [XmlEnum(Name = "0")]
        SEFAZ = 0,

        [XmlEnum(Name = "1")]
        JusticaFederal = 1,

        [XmlEnum(Name = "2")]
        JusticaEstadual = 2,

        [XmlEnum(Name = "3")]
        SECEX_RFB = 3,

        [XmlEnum(Name = "9")]
        Outros = 9
    }
}
