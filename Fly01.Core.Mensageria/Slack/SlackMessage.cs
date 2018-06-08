using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core.Mensageria.Slack
{
    public class SlackMessage
    {
        [JsonProperty("icon_emoji")]
        public string IconEmoji  { get; set; }

        [JsonProperty("attachments")]
        public List<SlackAttachment> Attachments { get; set; }

        public SlackMessage()
        {
            IconEmoji = ":robot_face:";
        }
    }
}