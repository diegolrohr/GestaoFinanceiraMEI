using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ResponseConsultaTotalStone
    {
        [JsonProperty("negative_balance")]
        public int SaldoDevedorCentavos { get; set; }
        
        /// <summary>
        /// Manter(100.00) double para correta conversão dos centavos
        /// </summary>
        [JsonIgnore]
        public double SaldoDevedor
        {
            get
            {
                return (SaldoDevedorCentavos / 100.00);
            }
        }

        [JsonProperty("total_prepayable_amount")]
        public int TotalBrutoCentavos { get; set; }

        [JsonIgnore]
        public double TotalBrutoAntecipavel
        {
            get
            {
                return (TotalBrutoCentavos / 100.00);
            }
        }
    }
}
