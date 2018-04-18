﻿using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System;
using Fly01.Core.Mensageria;
using Fly01.Core.Entities.Domains;

namespace Fly01.Core.ServiceBus
{
    public class Producer<TEntity> where TEntity : DomainBase
    {
        public static void Send(string routingKey, TEntity message, RabbitConfig.enHTTPVerb httpVerb)
        {
            try
            {
                using (var connection = RabbitConfig.Factory.CreateConnection("prdc" + RabbitConfig.QueueName))
                {
                    using (var channel = connection.CreateModel())
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        properties.AppId = RabbitConfig.AppId;
                        properties.Type = httpVerb.ToString();

                        channel.BasicPublish(RabbitConfig.AMQPExchange, routingKey, properties, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
                    }
                }
            }
            catch (Exception ex)
            {
                SlackClient.PostMessageErrorRabbit(
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)).ToString(), 
                    ex.Message, 
                    ex.StackTrace,
                    RabbitConfig.Factory?.VirtualHost,
                    RabbitConfig.QueueName);
            }
        }
    }
}