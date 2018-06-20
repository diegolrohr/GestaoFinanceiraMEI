using Fly01.Core.Mensageria;
using RabbitMQ.Client;
using System;

namespace Fly01.Core.ServiceBus
{
    public static class SetupEnvironment
    {
        public static void Create()
        {
            try
            {
                using (var connection = RabbitConfig.Factory.CreateConnection("env" + RabbitConfig.QueueName))
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(RabbitConfig.AMQPExchange, ExchangeType.Direct, true);
                        channel.QueueDeclare(RabbitConfig.QueueName, true, false, false, null);

                        RabbitConfig.ListRoutingKeys.ForEach(routingKey => { channel.QueueBind(RabbitConfig.QueueName, RabbitConfig.AMQPExchange, routingKey, null); });
                    }
                }
            }
            catch (Exception ex)
            {
                SlackClient.PostErrorRabbitMQ($"CRIAÇÃO DO AMBIENTE {RabbitConfig.QueueName}", ex, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, RabbitConfig.PlataformaUrl);
            }
        }
    }
}