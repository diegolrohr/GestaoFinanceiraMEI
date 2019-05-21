using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class SocketPlatformAppVM
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("actionUrl")]
        public string ActionUrl { get; set; }
    }
}