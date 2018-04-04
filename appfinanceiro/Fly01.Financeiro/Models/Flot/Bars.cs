using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Flot
{
    public class Bars
    {
        [JsonProperty("show")]
        public bool? Show { get; set; }

        [JsonProperty("align")]
        public string Align { get; set; }

        [JsonProperty("barWidth")]
        public decimal? BarWidth { get; set; }
    }
}