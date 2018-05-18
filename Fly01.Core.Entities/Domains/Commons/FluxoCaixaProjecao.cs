using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class FluxoCaixaProjecao
    {
        [JsonIgnore]
        public DateTime Data { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("totalRecebimentos")]
        public double TotalRecebimentos { get; set; }

        [JsonProperty("totalPagamentos")]
        public double TotalPagamentos { get; set; }

        [JsonProperty("saldoFinal")]
        public double SaldoFinal { get; set; }
    }
}