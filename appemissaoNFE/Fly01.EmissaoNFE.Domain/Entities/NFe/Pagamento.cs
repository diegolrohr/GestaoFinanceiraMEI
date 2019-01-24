using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("pag")]
    public class Pagamento
    {
        /// <summary>
        /// Detalhes dos pagamentos
        /// </summary>
        /// 
        [JsonProperty("detPag")]
        [XmlElement(ElementName = "detPag")]
        public List<DetalhePagamento> DetalhesPagamentos { get; set; }

        /// <summary>
        /// Informar o valor do Troco caso exista.
        /// O valor do Troco na NFe deve ser informado quando o valor da NFe (campo vNF)
        /// for diferente dos valores dos Pagamentos(campos vPag).
        /// </summary>
        [XmlIgnore]
        public double? ValorTroco { get; set; }

        [JsonProperty("vTroco_Opc")]
        [XmlElement(ElementName = "vTroco_Opc")]
        public string ValorTrocoString
        {
            get { return ValorTroco.HasValue ? ValorTroco.Value.ToString("0.00").Replace(",", ".") : null; }
            set { ValorTroco = double.Parse(value); }
        }

        public bool ShouldSerializeValorTrocoString()
        {
            return !string.IsNullOrEmpty(ValorTrocoString);
        }
    }
}
