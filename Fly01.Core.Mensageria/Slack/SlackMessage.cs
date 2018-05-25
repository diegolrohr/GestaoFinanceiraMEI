using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core.Mensageria.Slack
{
    public class SlackMessage
    {
        [JsonProperty("attachments")]
        public List<SlackAttachment> Attachments { get; set; }
    }
}