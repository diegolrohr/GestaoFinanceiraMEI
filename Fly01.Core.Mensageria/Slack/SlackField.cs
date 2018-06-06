using Newtonsoft.Json;

namespace Fly01.Core.Mensageria.Slack
{
    public class SlackField
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("short")]
        public bool Short { get; set; }

        public SlackField() { }
        public SlackField(string title, string value, bool shortValue = false)
        {
            Title = title;
            Value = value;
            Short = shortValue;
        }
    }
}
