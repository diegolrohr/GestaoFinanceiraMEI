using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.GoogleGeoChart
{
    public class Options
    {
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("resolution")]
        public string Resolution { get; set; }
        [JsonProperty("width")]
        public int? Width { get; set; }
        [JsonProperty("height")]
        public int? Height { get; set; }
        [JsonProperty("legend")]
        public Legend Legend { get; set; }
    }
}