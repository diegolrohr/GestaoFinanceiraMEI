using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Xml.Serialization;
using Fly01.Core;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    [XmlRoot(ElementName = "pagamento")]
    public class Pagamento
    {
        [XmlElement(ElementName = "parcela")]
        public int NumeroParcela { get; set; }

        [XmlIgnore]
        public DateTime DataVencimento { get; set; }

        [XmlElement(ElementName = "dtvenc")]
        public string DataVencimentoString
        {
            get { return DataVencimento.ToString("yyyy-MM-dd"); }
            set { DataVencimento = DateTime.Parse(value); }
        }

        [XmlIgnore]
        public double Valor { get; set; }

        /// <summary>
        /// Valor do pagamento, 15,2
        /// </summary>
        [XmlElement(ElementName = "valor")]
        public string ValorString
        {
            get { return Valor.ToString("0.00").Replace(",", "."); }
            set { Valor = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
    }
}
