using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ResponseAntecipacaoStoneVM
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("prepay_rate")]
        public double TaxaPontual { get; set; }

        [JsonProperty("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonProperty("liquidoAntecipar")]
        public double LiquidoAntecipar { get; set; }

        [JsonProperty("saldoLiquidoDisponivel")]
        public double SaldoLiquidoDisponivel { get; set; }
    }
}
