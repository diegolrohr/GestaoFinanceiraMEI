using Newtonsoft.Json;

namespace Fly01.Core.Entities.ViewModels
{
    public class AppsInstaladoslataformaVM
    {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("iconClass")]
        public string IconClass { get; set; }

        [JsonProperty("spanLabel")]
        public string SpanLabel { get; set; }

        [JsonProperty("urlTarget")]
        public string UrlTarget { get; set; }
    }
}