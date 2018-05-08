using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("pag")]
    public class Pagamento
    {
        /// <summary>
        /// Detalhe do pagamento
        /// </summary>
        [XmlElement(ElementName = "detPag")]
        public DetalhePagamento DetalhePagamento { get; set; }

        /// <summary>
        /// Informar o valor do Troco caso exista.
        /// </summary>
        [XmlIgnore]
        public double? ValorTroco { get; set; }

        [XmlElement(ElementName = "vTroco_Opc")]
        public string ValorTrocoString
        {
            get { return ValorTroco.Value.ToString("0.00").Replace(",", "."); }
            set { ValorTroco = double.Parse(value); }
        }

        public bool ShouldSerializeValorTrocoString()
        {
            return !string.IsNullOrEmpty(ValorTrocoString);
        }
    }
}
