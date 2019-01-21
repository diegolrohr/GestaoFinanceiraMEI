using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class AntecipacaoStone : StoneTokenBase
    {
        [JsonIgnore]
        public double Valor { get; set; }

        /// <summary>
        /// Valor sempre em centavos
        /// </summary>
        [JsonProperty("amount")]
        public int ValorCentavos
        {
            get
            {
                return (int)(Valor * 100);
            }
            set { }
        }

        [JsonProperty("minimumDays")]
        public int DiasMinimos
        {
            get
            {
                return 0;
            }
            set { }
        }

        /// <summary>
        /// 0: Undefined 1: Short 2: LongDistributed 3: ShortDistributed 4: Distributed
        /// enviar sempre 1
        /// </summary>
        [JsonProperty("transactionPickerType")]
        public int Modalidade
        {
            get
            {
                return 1;
            }
            set { }
        }
    }
}
