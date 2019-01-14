using Newtonsoft.Json;

namespace Fly01.Core.Presentation.Commons
{
    public class AppVM
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("accessUrl")]
        public string AccessUrl { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("cor")]
        public string Cor { get; set; }
    }
}
