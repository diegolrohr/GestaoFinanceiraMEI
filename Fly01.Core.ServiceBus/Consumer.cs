using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using Fly01.Core.Mensageria;
using System.Collections.Generic;

namespace Fly01.Core.ServiceBus
{
    public abstract class Consumer
    {
        private string MsgHeaderInvalid = "A 'PlataformaUrl' e o 'AppUser' devem ser informados no Header da request";

        private IConnection _connection;
        protected string Message;
        protected RabbitConfig.enHTTPVerb HTTPMethod;
        protected Dictionary<string, object> Headers = new Dictionary<string, object>();
        protected abstract Task PersistMessage();
                
        private IConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = RabbitConfig.Factory.CreateConnection("cnsm" + RabbitConfig.QueueName);

                return _connection;
            }
        }

        private IModel Channel
        {
            get
            {
                var model = Connection.CreateModel();
                return model;
            }
        }

        private bool HeaderIsValid()
        {
            return !string.IsNullOrWhiteSpace(GetHeaderValue("PlataformaUrl")) &&
                !string.IsNullOrWhiteSpace(GetHeaderValue("AppUser"));
        }

        private string GetHeaderValue(string key)
        {
            if(!Headers.ContainsKey(key))
                return string.Empty;

            return Encoding.UTF8.GetString(Headers[key] as byte[]);
        }
        
        public void Consume()
        {
            var consumer = new EventingBasicConsumer(Channel);
            
            consumer.Received += async (sender, args) =>
            {
                if (args.BasicProperties.AppId != RabbitConfig.AppId)
                {
                    try
                    {
                        if (args.BasicProperties.Headers == null)
                            throw new ArgumentException(MsgHeaderInvalid);

                        Headers = new Dictionary<string, object>(args.BasicProperties.Headers);
                        if (!HeaderIsValid())
                            throw new ArgumentException(MsgHeaderInvalid);

                        Message = Encoding.UTF8.GetString(args.Body);
                        HTTPMethod = (RabbitConfig.enHTTPVerb)Enum.Parse(typeof(RabbitConfig.enHTTPVerb), args.BasicProperties?.Type ?? "PUT");

                        RabbitConfig.PlataformaUrl = GetHeaderValue("PlataformaUrl");
                        RabbitConfig.AppUser = GetHeaderValue("AppUser");
                        RabbitConfig.RoutingKey = args.RoutingKey ?? string.Empty;

                        await PersistMessage();
                    }
                    catch (Exception ex)
                    {
                        SlackClient.PostMessageErrorRabbit(Message,
                            ex.Message, 
                            ex.StackTrace, 
                            RabbitConfig.Factory?.VirtualHost,
                            RabbitConfig.QueueName
                            );
                        Channel.BasicNack(args.DeliveryTag, false, true);
                    }
                    finally
                    {
                        Channel.BasicAck(args.DeliveryTag, false);
                    }
                }
            };

            Channel.BasicConsume(RabbitConfig.QueueName, true, consumer);
        }
    }
}