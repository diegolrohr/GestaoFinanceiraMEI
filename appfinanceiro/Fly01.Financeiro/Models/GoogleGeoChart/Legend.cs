using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.GoogleGeoChart
{
    public class Legend
    {
        [JsonProperty("numberFormat")]
        public string NumberFormat { get; set; }
    }
}