using Newtonsoft.Json;

namespace Fly01.Core.Mensageria.Slack
{
    public class SlackAction
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public SlackAction() { }
        public SlackAction(string text, string type, string url)
        {
            Text = text;
            Type = type;
            Url = url;
        }
    }
}