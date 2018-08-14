using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fly01.Core.Notifications;
using Fly01.Core.Mensageria;
using System.Linq;
using System.Web.Configuration;

namespace Fly01.Core.ServiceBus
{
    public abstract class Consumer
    {
        private readonly string MsgHeaderInvalid = "A 'PlataformaUrl', o 'Hostname' e o 'AppUser' devem ser informados no Header da request";

        protected string Message { get; private set; }
        protected string RoutingKey { get; private set; }
        protected string AppId { get; private set; }
        protected string PlataformaUrl { get; private set; }
        protected string AppUser { get; private set; }
        protected RabbitConfig.EnHttpVerb HTTPMethod { get; private set; }

        protected Dictionary<string, object> Headers = new Dictionary<string, object>();
        protected List<KeyValuePair<string, object>> exceptions;

        private IModel GetChannel(string virtualHost)
        {
            //RabbitConfig.Factory.VirtualHost = virtualHost;
            var conn = new ConnectionFactory()
            {
                Uri = new Uri(WebConfigurationManager.AppSettings["RabbitAMQPUrl"]),
                UserName = WebConfigurationManager.AppSettings["RabbitUserName"],
                Password = WebConfigurationManager.AppSettings["RabbitPassword"],
                VirtualHost = virtualHost
            };
            //var conn = RabbitConfig.Factory.CreateConnection($"cnsmr_{virtualHost}_{RabbitConfig.QueueName}");

            var channel = conn.CreateConnection($"cnsmr_{virtualHost}_{RabbitConfig.QueueName}").CreateModel();
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

        public void Consume(string channelName)
        {
            //RabbitConfig.VirtualHost.Split(',').ToList().ForEach(item => { EventConsumer(GetChannel(item)); });
            EventConsumer(GetChannel(channelName));
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

                    if (GetHeaderValue("Hostname") == "israel" || GetHeaderValue("Hostname") == "follmann")
                    {
                        if (args.BasicProperties.AppId != RabbitConfig.AppId)
                        {
                            Message = Encoding.UTF8.GetString(args.Body);
                            HTTPMethod = (RabbitConfig.EnHttpVerb)Enum.Parse(typeof(RabbitConfig.EnHttpVerb), args.BasicProperties?.Type ?? "POST");
                            RoutingKey = args.RoutingKey ?? string.Empty;
                            AppId = args.BasicProperties.AppId;
                            PlataformaUrl = GetHeaderValue("PlataformaUrl");
                            AppUser = GetHeaderValue("AppUser");

                            await DeliverMessage();

                            foreach (var item in exceptions)
                            {
                                var erro = (item.Value is BusinessException) ? (BusinessException)item.Value : (Exception)item.Value;

                                SlackClient.PostErrorRabbitMQ(item.Key, erro, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, GetHeaderValue("PlataformaUrl"), args.RoutingKey);

                                continue;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SlackClient.PostErrorRabbitMQ("Erro RabbitMQ", ex, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, GetHeaderValue("PlataformaUrl"), args.RoutingKey);
                }
                finally
                {
                    channel.BasicAck(args.DeliveryTag, false);
                }
            };

            channel.BasicConsume(RabbitConfig.QueueName, false, consumer);
        }

        protected abstract Task DeliverMessage();
    }
}