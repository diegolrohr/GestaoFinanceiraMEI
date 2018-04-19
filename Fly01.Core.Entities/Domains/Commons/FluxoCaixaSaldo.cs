using Newtonsoft.Json;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class FluxoCaixaSaldo
    {
        [JsonProperty("saldoConsolidado")]
        public double SaldoConsolidado { get; set; }

        [JsonProperty("areceberhoje")]
        public double AReceberHoje { get; set; }

        [JsonProperty("apagarhoje")]
        public double APagarHoje { get; set; }
    }
}