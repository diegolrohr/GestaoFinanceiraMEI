using RabbitMQ.Client;

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
                        channel.QueueDeclare(RabbitConfig.QueueName + "_callback", true, false, false, null);

                        RabbitConfig.ListRoutingKeys.ForEach(routingKey => { channel.QueueBind(RabbitConfig.QueueName, RabbitConfig.AMQPExchange, routingKey, null); });
                        channel.QueueBind(RabbitConfig.QueueName + "_callback", RabbitConfig.AMQPExchange, "callback", null);
                    }
                }

            }
            catch (System.Exception ex)
            {
                SlackClient.PostMessageErrorRabbit($"CRIAÇÃO DO AMBIENTE {RabbitConfig.QueueName}", ex.Message, ex.StackTrace);
            }

            //var properties = channel.CreateBasicProperties();

            //properties.Headers = new Dictionary<string, object>();
            //properties.Headers.Add("latitude", 51.5252949);
        }
    }
}