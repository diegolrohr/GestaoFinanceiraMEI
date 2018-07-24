using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class DashboardComprasVM
    {
        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }
    }
}
