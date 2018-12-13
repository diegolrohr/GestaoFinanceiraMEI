using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ResponseConsultaTotalStone
    {
        [JsonProperty("negative_balance")]
        public int SaldoDevedorCentavos { get; set; }

        [JsonIgnore]
        public double SaldoDevedor
        {
            get
            {
                return (SaldoDevedorCentavos / 100);
            }
            set { }
        }
    }
}
