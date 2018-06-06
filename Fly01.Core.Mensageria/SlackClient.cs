using System.Net;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Collections.Generic;
using Fly01.Core.Notifications;
using System.Configuration;
using System;
using Fly01.Core.Mensageria.Slack;
using System.Data.Entity.Infrastructure;
using System.Text;

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

                client.UploadValuesAsync(new Uri(slackUrl), "POST", data);
            }
        }

        private static string GetCustomMessage(Exception exception)
        {
            var response = exception.Message;
            if (exception is DbUpdateException)
            {
                var sb = new StringBuilder();
                var inner = exception.InnerException;

                while (inner != null)
                {
                    sb.AppendFormat("{0}. ", inner.Message);
                    inner = inner.InnerException;
                }

                response = sb.ToString();
            }

            return response;
        }

        public static async void PostErrorRabbitMQ(string data, Exception exception, string hostName, string queueName, string plataformaUrl)
        {
            var slackChannel = string.Empty;
            var isProd = (hostName == "prod");
            var isHomolog = (hostName == "homolog");
            var errorMessage = GetCustomMessage(exception);

            slackChannel = isProd
                ? "https://hooks.slack.com/services/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"
                : "https://hooks.slack.com/services/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";

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

            if (isHomolog || isProd)
            {
                var mongoHelper = new LogMongoHelper<LogServiceBusEvent>(ConfigurationManager.AppSettings["MongoDBLog"]);
                var collection = mongoHelper.GetCollection(ConfigurationManager.AppSettings["MongoCollectionNameServiceBusLog"]);
                await collection.InsertOneAsync(new LogServiceBusEvent() { MessageData = data, Error = errorMessage, StackTrace = exception.StackTrace, Host = hostName, Queue = queueName, PlatformUrl = plataformaUrl });
            }

            Post(slackChannel, message);
        }
    }
}