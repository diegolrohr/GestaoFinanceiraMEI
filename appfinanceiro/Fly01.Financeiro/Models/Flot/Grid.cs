using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Flot
{
    public class Grid
    {
        [JsonProperty("hoverable")]
        public bool Hoverable { get; set; }
    }
}