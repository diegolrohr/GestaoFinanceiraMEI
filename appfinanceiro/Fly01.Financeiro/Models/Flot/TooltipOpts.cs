using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Flot
{
    public class TooltipOpts
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("xDateFormat")]
        public string XDateFormat { get; set; }
    }
}