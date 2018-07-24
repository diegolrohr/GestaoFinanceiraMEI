using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ComprasStatusVM
    {
        [JsonProperty("tipo")]
        public string Status { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
}
