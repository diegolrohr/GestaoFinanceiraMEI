using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fly01.Core.Notifications;
using Fly01.Core.Mensageria;
using System.Linq;

namespace Fly01.Core.ServiceBus
{
    public abstract class Consumer
    {
        private string MsgHeaderInvalid = "A 'PlataformaUrl', o 'Hostname' e o 'AppUser' devem ser informados no Header da request";

        protected string Message;
        protected RabbitConfig.EnHttpVerb HTTPMethod;
        protected Dictionary<string, object> Headers = new Dictionary<string, object>();
        protected List<KeyValuePair<string, object>> exceptions = new List<KeyValuePair<string, object>>();

        private IModel GetChannel(string virtualHost)
        {
            RabbitConfig.Factory.VirtualHost = virtualHost;
            var conn = RabbitConfig.Factory.CreateConnection($"cnsmr_{virtualHost}_{RabbitConfig.QueueName}");
            var channel = conn.CreateModel();
            channel.BasicQos(0, 1, false);

            return channel;
        }

        private bool HeaderIsValid()
        {
            return !string.IsNullOrWhiteSpace(GetHeaderValue("PlataformaUrl")) &&
                !string.IsNullOrWhiteSpace(GetHeaderValue("AppUser")) &&
                !string.IsNullOrWhiteSpace(GetHeaderValue("Hostname"));
        }

        private string GetHeaderValue(string key)
        {
            if (!Headers.ContainsKey(key))
                return string.Empty;

            return Encoding.UTF8.GetString(Headers[key] as byte[]);
        }

        public void Consume()
        {
            RabbitConfig.VirtualHost.Split(',').ToList().ForEach(item => { EventConsumer(GetChannel(item)); });
        }

        private void EventConsumer(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, args) =>
            {
                try
                {
                    if (args.BasicProperties.Headers == null)
                        throw new ArgumentException(MsgHeaderInvalid);

                    Headers = new Dictionary<string, object>(args.BasicProperties.Headers);
                    if (!HeaderIsValid())
                        throw new ArgumentException(MsgHeaderInvalid);

                    if (GetHeaderValue("Hostname") == RabbitConfig.VirtualHostname || GetHeaderValue("Hostname") == "integracao")
                    {
                        if (args.BasicProperties.AppId != RabbitConfig.AppId)
                        {
                            Message = Encoding.UTF8.GetString(args.Body);
                            HTTPMethod = (RabbitConfig.EnHttpVerb)Enum.Parse(typeof(RabbitConfig.EnHttpVerb), args.BasicProperties?.Type ?? "POST");

                            RabbitConfig.PlataformaUrl = GetHeaderValue("PlataformaUrl");
                            RabbitConfig.AppUser = GetHeaderValue("AppUser");
                            RabbitConfig.RoutingKey = args.RoutingKey ?? string.Empty;
                            
                            await DeliverMessage(args.BasicProperties.AppId);

                            foreach (var item in exceptions)
                            {
                                var erro = (item.Value is BusinessException) ? (BusinessException)item.Value : (Exception)item.Value;

                                SlackClient.PostErrorRabbitMQ(item.Key, erro, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, RabbitConfig.PlataformaUrl, RabbitConfig.RoutingKey);

                                continue;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SlackClient.PostErrorRabbitMQ("Erro RabbitMQ", ex, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, RabbitConfig.PlataformaUrl, RabbitConfig.RoutingKey);
                }
                finally
                {
                    channel.BasicAck(args.DeliveryTag, false);
                }
            };

            channel.BasicConsume(RabbitConfig.QueueName, false, consumer);
        }

        protected abstract Task DeliverMessage(string appId);
    }
}