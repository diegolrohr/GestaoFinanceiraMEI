using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Flot
{
    public class Axis
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }
        [JsonProperty("tickFormatter")]
        public string TickFormatter { get; set; }
        [JsonProperty("tickFormatterData")]
        public Dictionary<string, string> TickFormatterData { get; set; }
        [JsonProperty("ticks")]
        public List<object[]> Ticks { get; set; }
        [JsonProperty("min")]
        public int? Min { get; set; }
        [JsonProperty("max")]
        public int? Max { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
        [JsonProperty("minTickSize")]
        public int? MinTickSize { get; set; }
    }
}