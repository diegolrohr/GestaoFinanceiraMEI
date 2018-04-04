using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class II
    {
        /// <summary>
        /// Valor da BC do Imposto de Importação
        /// </summary>
        [XmlElement(ElementName = "vBC")]
        public double ValorBC { get; set; }

        /// <summary>
        /// valor das despesas aduaneiras
        /// </summary>
        [XmlElement(ElementName = "vDespAdu")]
        public double DespesasAduaneiras { get; set; }

        /// <summary>
        /// valor do Imposto de Importação
        /// </summary>
        [XmlElement(ElementName = "vII")]
        public double ValorII { get; set; }

        /// <summary>
        /// Valor do IOF - Imposto sobre Operações Financeiras
        /// </summary>
        [XmlElement(ElementName = "vIOF")]
        public double ValorIOF { get; set; }
    }
}
