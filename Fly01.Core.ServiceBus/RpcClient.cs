using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Threading;
using System.Web.Configuration;

namespace Fly01.Core.ServiceBus
{
    public class RpcClient
    {
        private readonly IModel channel;
        private readonly IConnection connection;
        private readonly IBasicProperties props;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();

        public RpcClient()
        {
            connection = RabbitConfig.Factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            var correlationId = Guid.NewGuid().ToString();
            props = channel.CreateBasicProperties();
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
            var messageBytes = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: WebConfigurationManager.AppSettings["RabbitSequenceGenetorQueueName"],
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            var result = "";
            respQueue.TryTake(out result, 2000);

            if (string.IsNullOrEmpty(result))
                throw new Exception("RpcClient: Não foi possível obter um número");
                
            return result;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}