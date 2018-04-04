using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Flot
{
    public class Series
    {
        [JsonProperty("lines")]
        public Lines Lines { get; set; }

        [JsonProperty("points")]
        public Points Points { get; set; }

        [JsonProperty("bars")]
        public Bars Bars { get; set; }
    }
}