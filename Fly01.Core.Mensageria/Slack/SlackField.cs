using Newtonsoft.Json;

namespace Fly01.Core.Mensageria.Slack
{
    public class SlackField
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }

        public SlackField() { }
        public SlackField(string title, string value)
        {
            Title = title;
            Value = value;
        }
    }
}
