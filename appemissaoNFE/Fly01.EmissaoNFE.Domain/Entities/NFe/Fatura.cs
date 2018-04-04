using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot(ElementName = "fat")]
    public class Fatura
    {
        /// <summary>
        /// informar o número da fatura
        /// </summary>
        [XmlElement(ElementName = "nFat")]
        public string NumeroFatura { get; set; }

        /// <summary>
        /// informar o valor originário da fatura
        /// </summary>
        [XmlElement(ElementName = "vOrig", IsNullable = true)]
        public double? ValorOiriginarioFatura { get; set; }

        public bool ShouldSerializeValorOiriginarioFatura()
        {
            return ValorOiriginarioFatura.HasValue & ValorOiriginarioFatura > 0;
        }

        /// <summary>
        /// informar o valor do desconto
        /// </summary>
        [XmlElement(ElementName = "vDesc", IsNullable = true)]
        public double? ValorDesconto { get; set; }

        public bool ShouldSerializeValorDesconto()
        {
            return ValorDesconto.HasValue & ValorDesconto > 0;
        }

        /// <summary>
        /// informar o valor Liquido da fatura
        /// </summary>
        [XmlElement(ElementName = "vLiq", IsNullable = true)]
        public double? ValorLiquidoFatura { get; set; }

        public bool ShouldSerializeValorLiquidoFatura()
        {
            return ValorLiquidoFatura.HasValue & ValorLiquidoFatura > 0;
        }
    }
}
