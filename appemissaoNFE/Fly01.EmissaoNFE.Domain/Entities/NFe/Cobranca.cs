using Fly01.EmissaoNFE.Domain.Entities.NFe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("cobr")]
    public class Cobranca
    {
        /// <summary>
        /// informar o grupo de Fatura
        /// </summary>
        [XmlElement(ElementName = "fat")]
        public Fatura[] Fatura { get; set; }

        /// <summary>
        /// informar o grupo dup de duplicatas
        /// </summary>
        [XmlElement(ElementName = "dup")]
        public Duplicata[] Duplicata { get; set; }

    }
}
