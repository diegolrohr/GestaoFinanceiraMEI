using Newtonsoft.Json;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class FluxoCaixaSaldo
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