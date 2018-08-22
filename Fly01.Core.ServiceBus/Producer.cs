using System;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Fly01.Core.Mensageria;
using Fly01.Core.Entities.Domains;
using System.Collections.Generic;

namespace Fly01.Core.ServiceBus
{
    public class Producer<TEntity> where TEntity : DomainBase
    {
        public static void Send(string routingKey, string appUser, string plataformaUrl, TEntity message, RabbitConfig.EnHttpVerb httpVerb)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = RabbitConfig.AMQPURL,
                    UserName = RabbitConfig.UserName,
                    Password = RabbitConfig.Password,
                    VirtualHost = RabbitConfig.VirtualHostApps
                };

                using (var connection = factory.CreateConnection("prdc" + RabbitConfig.QueueName))
                {
                    var channel = connection.CreateModel();

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.AppId = RabbitConfig.AppId;
                    properties.Type = httpVerb.ToString();

                    properties.Headers = new Dictionary<string, object>()
                    {
                        { "AppUser", appUser },
                        { "PlataformaUrl", plataformaUrl },
                        { "Hostname", RabbitConfig.VirtualHostApps },
                    };

                    if (RabbitConfig.IsDevEnvironment) routingKey = Environment.MachineName +"_"+ routingKey;

                    channel.BasicPublish(RabbitConfig.AMQPExchange, routingKey, properties, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
                }
            }
            catch (Exception ex)
            {
                    var _mediaClient = new MediaClient();
                _mediaClient.PostErrorRabbitMQ(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)).ToString(), ex, RabbitConfig.VirtualHostApps, RabbitConfig.QueueName, plataformaUrl, routingKey);
            }
        }
    }
}