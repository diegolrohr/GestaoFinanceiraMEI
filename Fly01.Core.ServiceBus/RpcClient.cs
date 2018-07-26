using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;

namespace Fly01.Core.ServiceBus
{
    public class RpcClient
    {
        private static IModel channel;
        private static IConnection connection;
        private readonly IBasicProperties props;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();

        private IConnection Connection
        {
            get
            {
                if (connection == null || !connection.IsOpen)
                {
                    var factory = RabbitConfig.Factory;
                    factory.VirtualHost = RabbitConfig.VirtualHostname;

                    connection = factory.CreateConnection();
                }

                return connection;
            }
        }

        private IModel Channel
        {
            get
            {
                if (channel == null || channel.IsClosed)
                    channel = Connection.CreateModel();

                return channel;
            }
        }

        public RpcClient()
        {
            var myChannel = Channel;
            replyQueueName = myChannel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(myChannel);

            var correlationId = Guid.NewGuid().ToString();
            props = myChannel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                    respQueue.Add(Encoding.UTF8.GetString(ea.Body));
            };
        }

        public string Call(string message)
        {
            var result = "";

            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: RabbitConfig.SequenceGeneratorQueue, basicProperties: props, body: messageBytes);

                channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);

                respQueue.TryTake(out result, 2000);

                if (string.IsNullOrEmpty(result))
                    throw new Exception("RpcClient: Não foi possível obter um número");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                channel.QueueDelete(replyQueueName, false, true);
            }

            return result;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}