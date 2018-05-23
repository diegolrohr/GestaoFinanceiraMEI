using System.Net;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Fly01.Core.Mensageria
{
    public static class SlackClient
    {
        public static void Post(string slackUrl, SlackMessage message)
        {
            using (var client = new WebClient())
            {
                var data = new NameValueCollection
                {
                    ["payload"] = JsonConvert.SerializeObject(message)
                };

                client.UploadValues(slackUrl, "POST", data);
            }
        }

        public static void PostErrorRabbitMQ(string data, string errorMessage, string hostName, string queueName)
        {
            var slackChannel = string.Empty;
            if (hostName == "prod")
                slackChannel = "https://hooks.slack.com/services/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";
            else
                slackChannel = "https://hooks.slack.com/services/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";

            var message = new SlackMessage()
            {
                Attachments = new List<SlackAttachment>()
                {
                    new SlackAttachment()
                    {
                        Fallback = "Required plain-text summary of the attachment.",
                        Color = "danger",
                        Title = "Erro Rabbit MQ",
                        Fields = new List<SlackField>()
                        {
                            new SlackField("Dados", data),
                            new SlackField("Erro", errorMessage),
                            new SlackField("Host", hostName),
                            new SlackField("Fila", queueName),
                        }
                    }
                }
            };

            Post(slackChannel, message);
        }
    }
    
    public class SlackMessage
    {
        [JsonProperty("attachments")]
        public List<SlackAttachment> Attachments { get; set; }
    }

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