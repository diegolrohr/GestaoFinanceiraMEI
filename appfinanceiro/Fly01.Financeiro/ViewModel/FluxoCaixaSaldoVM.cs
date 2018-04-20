using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    public class ResponseFluxoCaixaSaldoVM
    {
        [JsonProperty("value")]
        public FluxoCaixaSaldoVM Value { get; set; }
    }

    public class FluxoCaixaSaldoVM
    {
        [JsonProperty("saldoConsolidado")]
        public double SaldoConsolidado { get; set; }

        [JsonProperty("areceberhoje")]
        public double AReceberHoje { get; set; }

        [JsonProperty("apagarhoje")]
        public double APagarHoje { get; set; }
    }
}