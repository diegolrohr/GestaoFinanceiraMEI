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
        [JsonProperty("saldoAtual")]
        public double SaldoAtual { get; set; }

        [JsonProperty("totalRecebimentos")]
        public double TotalRecebimentos { get; set; }

        [JsonProperty("totalPagamentos")]
        public double TotalPagamentos { get; set; }

        [JsonProperty("saldoProjetado")]
        public double SaldoProjetado { get; set; }
    }
}