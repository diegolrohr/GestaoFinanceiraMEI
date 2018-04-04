using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Flot
{
    public class Lines
    {
        [JsonProperty("show")]
        public bool Show { get; set; }
    }
}