using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Flot
{
    public class Options
    {
        [JsonProperty("series")]
        //[DataMember]
        public Series Series { get; set; }

        [JsonProperty("xaxis")]
        public Axis XAxis { get; set; }

        [JsonProperty("yaxis")]
        public Axis YAxis { get; set; }

        [JsonProperty("tooltip")]
        public bool Tooltip { get; set;}

        [JsonProperty("tooltipOpts")]
        public TooltipOpts TooltipOpts { get; set; }

        [JsonProperty("grid")]
        public Grid Grid { get; set; }

        [JsonProperty("bars")]
        public Bars Bars { get; set; }
    }
}