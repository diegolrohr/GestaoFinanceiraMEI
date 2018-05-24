using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core.Mensageria.Slack
{
    public class SlackAttachment
    {
        [JsonProperty("fallback")]
        public string Fallback { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("fields")]
        public List<SlackField> Fields { get; set; }
    }
}
