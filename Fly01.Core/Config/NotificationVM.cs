using Newtonsoft.Json;

namespace Fly01.Core.Config
{
    public class NotificationVM
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
