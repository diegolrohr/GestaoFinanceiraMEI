using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Flot
{
    public class Points
    {
        [JsonProperty("show")]
        public bool Show { get; set; }
    }
}