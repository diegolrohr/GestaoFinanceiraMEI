using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot(ElementName = "dup")]
    public class Duplicata
    {
        /// <summary>
        /// informar o número da duplicata
        /// </summary>
        [XmlElement(ElementName = "nDup")]
        public string NumeroDuplicata { get; set; }

        #region dateTime

        /// <summary>
        /// informar a data de vencimento da duplicata como DateTime
        /// </summary>
        [XmlIgnore]
        public DateTime VencimentoDuplicata { get; set; }

        /// <summary>
        /// informar a data de vencimento da duplicata como string
        /// </summary>
        [XmlElement(ElementName = "dVenc")]
        public string VencimentoDuplicataString
        {
            get { return this.VencimentoDuplicata.ToString("yyyy-MM-ddTHH:mm:sszzzz"); }
            set { this.VencimentoDuplicata = DateTime.Parse(value); }
        }

        #endregion dateTime

        /// <summary>
        /// informar o valor da duplicata
        /// </summary>
        [XmlElement(ElementName = "vDup", IsNullable = true)]
        public double? ValorDuplicata { get; set; }

        public bool ShouldSerializeValorDuplicata()
        {
            return ValorDuplicata.HasValue;
        }
    }
}
