using System.Net;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System;
using Fly01.Core.Mensageria.Slack;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Data.Entity.Validation;
using Fly01.Core.Entities.Domains.NoSQL;
using Fly01.Core.Helpers;

namespace Fly01.Core.Mensageria
{
    public static class SlackClient
    {
        private static void Post(string slackUrl, SlackMessage message)
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
            var sb = new StringBuilder();
            if (exception is DbUpdateException)
            {
                var inner = exception.InnerException;
                while (inner != null)
                {
                    sb.Append($"{inner.Message} .");

                    inner = inner.InnerException;
                }

                if (sb.Length > 0)
                    response = sb.ToString();
            }
            else if (exception is DbEntityValidationException)
            {
                foreach (var entityValidationErrors in ((DbEntityValidationException)exception).EntityValidationErrors)
                {
                    var entityName = entityValidationErrors.Entry.Entity.GetType().Name;
                    foreach (var itemValidationError in entityValidationErrors.ValidationErrors)
                        sb.Append($"Entity {entityName} : {itemValidationError.ErrorMessage} ({itemValidationError.PropertyName})");
                }

                if (sb.Length > 0)
                    response = sb.ToString();
            }

            return response;
        }

        private static string GetFlyEnvironmentUrl(Guid messageId, string routeName, string hostName)
        {
            var host = hostName == "prod" ? ""
                : hostName == "homolog" ? "dev"
                : "local";

            var url = $"http://gestao.fly01{host}.com.br/{routeName}/Edit/{messageId}";

            return url;
        }

        public static async void PostErrorRabbitMQ(string data, Exception exception, string hostName, string queueName, string plataformaUrl, string routingKey)
        {
            var slackChannel = string.Empty;
            var isProd = (hostName == "prod");
            var isHomolog = (hostName == "homolog");
            var errorMessage = GetCustomMessage(exception);

            slackChannel = isProd
                ? "https://totvs.ryver.com/application/webhook/YM0Hs5umn114vPb"
                : "https://totvs.ryver.com/application/webhook/bP5-1AzID58i0ai";

            var logData = new LogServiceBusEvent() { MessageData = data, Error = errorMessage, StackTrace = exception.StackTrace, Host = hostName, Queue = queueName, PlatformId = plataformaUrl, RoutingKey = routingKey };
            var message = new SlackMessage()
            {
                Attachments = new List<SlackAttachment>()
                {
                    new SlackAttachment()
                    {
                        Fallback = "Required plain-text summary of the attachment.",
                        Color = "danger",
                        Title = $"Erro ao processar mensagem no {queueName}",
                        Fields = new List<SlackField>()
                        {
                            new SlackField("Data", data),
                            new SlackField("Error", errorMessage),
                            new SlackField("Host", hostName, true),
                            new SlackField("Fila", queueName, true)
                        },
                        Actions = new List<SlackAction>()
                        {
                            new SlackAction("Ver mensagem no Fly", "button", GetFlyEnvironmentUrl(logData.Id, "LogRabbitMQ", hostName))
                        }
                    }
                }
            };

            if (isHomolog || isProd)
            {
                var mongoHelper = new LogMongoHelper<LogServiceBusEvent>(ConfigurationManager.AppSettings["MongoDBLog"]);
                var collection = mongoHelper.GetCollection(ConfigurationManager.AppSettings["MongoCollectionNameServiceBusLog"]);
                await collection.InsertOneAsync(logData);
            }

            Post(slackChannel, message);
        }

        public static void PostNotificacaoAvaliacaoApp(Guid mensagemId, string hostName, string mensagem, string plataforma)
        {
            var slackChannel = "https://hooks.slack.com/services/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";
            var message = new SlackMessage()
            {
                Attachments = new List<SlackAttachment>()
                {
                    new SlackAttachment()
                    {
                        Fallback = "Required plain-text summary of the attachment.",
                        Color = "info",
                        Title = "Mensagem recebida via avaliação de apps",
                        Fields = new List<SlackField>()
                        {
                            new SlackField("Mensagem", mensagem),
                            new SlackField("Plataforma", plataforma),
                        },
                        Actions = new List<SlackAction>()
                        {
                            new SlackAction("Ver mensagem no Fly", "button", $"http://gestao.{hostName}.com.br/LogAvaliacaoApp/Edit/{mensagemId}")
                        }
                    }
                }
            };

            Post(slackChannel, message);
        }
    }
}