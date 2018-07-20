using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ContasPagarDoDiaVM
    {
        [JsonProperty("vencimento")]
        public DateTime Vencimento { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
