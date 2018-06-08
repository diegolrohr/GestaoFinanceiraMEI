using Newtonsoft.Json;
using System;
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

        [JsonProperty("actions")]
        public List<SlackAction> Actions { get; set; }

        [JsonProperty("footer")]
        public string Footer { get; set; }

        [JsonProperty("footer_icon")]
        public string FooterIcon { get; set; }

        [JsonProperty("ts")]
        public double TimeStamp { get; set; }

        public SlackAttachment()
        {
            FooterIcon = "https://platform.slack-edge.com/img/default_application_icon.png";
            Footer = "Notification";
            TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}