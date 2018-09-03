using Fly01.Core.Mensageria;
using RabbitMQ.Client;
using System;
using System.Web.Configuration;

namespace Fly01.Core.ServiceBus
{
    public static class SetupEnvironment
    {
        public static void Create(string virtualHost)
        {
            try
            {
                if (virtualHost == "") return;

                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(WebConfigurationManager.AppSettings["RabbitAMQPUrl"]),
                    UserName = WebConfigurationManager.AppSettings["RabbitUserName"],
                    Password = WebConfigurationManager.AppSettings["RabbitPassword"],
                    VirtualHost = virtualHost
                };

                using (var connection = factory.CreateConnection("env" + RabbitConfig.QueueName))
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(RabbitConfig.AMQPExchange, ExchangeType.Direct, true);
                        channel.QueueDeclare(RabbitConfig.QueueName, true, false, false, null);

                        if (RabbitConfig.IsDevEnvironment)
                            RabbitConfig.ListRoutingKeys.ForEach(routingKey => { channel.QueueBind(RabbitConfig.QueueName, RabbitConfig.AMQPExchange, $"{Environment.MachineName}_{routingKey}", null); });
                        else
                            RabbitConfig.ListRoutingKeys.ForEach(routingKey => { channel.QueueBind(RabbitConfig.QueueName, RabbitConfig.AMQPExchange, routingKey, null); });
                    }
                }
            }
            catch (Exception ex)
            {
                var _mediaClient = new MediaClient();
                _mediaClient.PostErrorRabbitMQ($"CRIAÇÃO DO AMBIENTE {RabbitConfig.QueueName}", ex, virtualHost, RabbitConfig.QueueName, "", "");
            }
        }
    }
}