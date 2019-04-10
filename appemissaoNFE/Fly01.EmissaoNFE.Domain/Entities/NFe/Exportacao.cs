using Fly01.EmissaoNFE.Domain.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Exportacao
    {
        /// <summary>
        /// informar a sigla da UF de Embarque ou de transposição de fronteira. A UF de embarque é a UF do local onde será embarcada para o exterior (porto/aeroporto), 
        /// no caso de ser transporte terrestre deve ser o local de transposição de fronteira. Não aceita o valor "EX".
        /// </summary>
        [XmlElement(ElementName = "UFSaidaPais")]
        public string UFSaidaPais { get; set; }

        /// <summary>
        /// informar o local de embarque, local onde será embarcada para o exterior (porto/aeroporto), no caso de ser transporte terrestre deve ser 
        /// o local de transposição de fronteira.
        /// </summary>
        [XmlElement(ElementName = "xLocEmbarq")]
        public string xLocEmbarq { get; set; }
    }
}
