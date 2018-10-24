using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    [XmlRoot(ElementName = "fatura")]
    public class Fatura
    {
        [XmlElement(ElementName = "numero")]
        public int Numero { get; set; }

        [XmlIgnore]
        public double Valor { get; set; }

        /// <summary>
        /// Valor da fatura do documento
        /// </summary>
        [XmlElement(ElementName = "valor")]
        public string ValorString
        {
            get { return Valor.ToString("0.00").Replace(",", "."); }
            set { Valor = double.Parse(value.Replace(".", ",")); }
        }
    }
}
