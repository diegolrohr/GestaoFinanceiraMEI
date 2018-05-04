using System;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using Fly01.Core.Mensageria;

namespace Fly01.Core.ServiceBus
{
    public abstract class Consumer
    {
        private string Username
        {
            get
            {
                var userName = string.Empty;
                if (string.IsNullOrEmpty(Message))
                    return userName;

                userName =
                    HTTPMethod == RabbitConfig.enHTTPVerb.POST ? JObject.Parse(Message).GetValue("usuarioInclusao")?.ToString()
                    : HTTPMethod == RabbitConfig.enHTTPVerb.PUT ? JObject.Parse(Message).GetValue("usuarioAlteracao")?.ToString()
                    : JObject.Parse(Message).GetValue("usuarioExclusao")?.ToString();

                return userName;
            }
        }

        private string Plataforma
        {
            get
            {
                if (string.IsNullOrEmpty(Message))
                    return string.Empty;

                return JObject.Parse(Message).GetValue("plataformaId")?.ToString();
            }
        }

        private IConnection conn;
        private IConnection Conn
        {
            get
            {
                if (conn == null)
                    conn = RabbitConfig.Factory.CreateConnection("cnsm" + RabbitConfig.QueueName);

                return conn;
            }
        }

        private IModel channel
        {
            get
            {
                var model = Conn.CreateModel();
                return model;
            }
        }

        protected string Message;
        protected RabbitConfig.enHTTPVerb HTTPMethod;

        public void Consume()
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, ea) =>
            {
                if (ea.BasicProperties.AppId != RabbitConfig.AppId)
                {
                    var properties = ea.BasicProperties;

                    try
                    {
                        Message = Encoding.UTF8.GetString(ea.Body);
                        HTTPMethod = (RabbitConfig.enHTTPVerb)Enum.Parse(typeof(RabbitConfig.enHTTPVerb), ea.BasicProperties?.Type ?? "PUT");
                        
                        RabbitConfig.PlataformaUrl = Plataforma;
                        RabbitConfig.AppUser = Username;
                        RabbitConfig.RoutingKey = ea.RoutingKey ?? "";

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
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                    finally
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            };

            channel.BasicConsume(RabbitConfig.QueueName, true, consumer);
        }

        protected abstract Task PersistMessage();
    }
}