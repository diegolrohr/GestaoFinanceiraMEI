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
                    var factory = new ConnectionFactory()
                    {
                        Uri = RabbitConfig.AMQPURL,
                        UserName = RabbitConfig.UserName,
                        Password = RabbitConfig.Password,
                        VirtualHost = RabbitConfig.VirtualHostApps,
                    };

                    connection = factory.CreateConnection("cnsmr_rpc");
                }

                return connection;
                //return factory.CreateConnection("cnsmr_rpc");
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
            lock (myChannel)
            {
                try
                {

                    replyQueueName = myChannel.QueueDeclare().QueueName;
                    consumer = new EventingBasicConsumer(myChannel);
                    props = myChannel.CreateBasicProperties();
                    var correlationId = Guid.NewGuid().ToString();
                    props.CorrelationId = correlationId;
                    props.ReplyTo = replyQueueName;

                    consumer.Received += (model, ea) =>
                    {
                        if (ea.BasicProperties.CorrelationId == correlationId)
                            respQueue.Add(Encoding.UTF8.GetString(ea.Body));
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception($"RPC: {ex.Message}");
                }
            }
        }

        public string Call(string message)
        {
            var result = string.Empty;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: RabbitConfig.SequenceGeneratorQueue, basicProperties: props, body: messageBytes);

            lock (channel)
            {
                try
                {
                    channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);
                    respQueue.TryTake(out result, 2000);

                    if (string.IsNullOrEmpty(result))
                        throw new Exception($"RPC: Não foi possível obter um número para {message}");

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"RPC: {ex.Message}");
                }
                finally
                {
                    //channel.BasicCancel(channelTag);
                    channel.QueueDelete(replyQueueName, false, true);
                }
            }
        }

        public void Close()
        {
            //channel.BasicCancel(consumer.ConsumerTag);
            //channel.QueueDelete(replyQueueName, false, true);
            //connection.Close();
        }
    }
}