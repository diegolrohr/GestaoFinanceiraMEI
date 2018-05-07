using System.Net;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace Fly01.Core.Mensageria
{
    public static class SlackClient
    {
        public static void PostMessageErrorRabbit(string data, string errorMessage, string errorStackTrace, string hostName, string queueName)
        {
            var slackChannel = "";

            if (hostName == "prod")
                slackChannel = "https://hooks.slack.com/services/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";
            else
                slackChannel = "https://hooks.slack.com/services/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";

            using (var client = new WebClient())
            {
                var message = JsonConvert.SerializeObject(new
                {
                    attachments = new[]{ new
                        {
                            fallback = "Required plain-text summary of the attachment.",
                            color = "danger",
                            title = "Erro Rabbit MQ",
                            fields = new[]
                            {
                                new
                                {
                                    title = "Dados",
                                    value = data
                                },
                                new
                                {
                                    title = "Erro",
                                    value = errorMessage
                                },
                                //new
                                //{
                                //    title = "Stack Trace",
                                //    value = errorStackTrace
                                //},
                                new {
                                    title = "Host",
                                    value = hostName
                                },
                                new
                                {
                                    title = "Fila",
                                    value = queueName
                                }
                            }
                        }
                    }
                });

                var dados = new NameValueCollection { ["payload"] = message };

                client.UploadValues(slackChannel, "POST", dados);
            }
        }
    }
}