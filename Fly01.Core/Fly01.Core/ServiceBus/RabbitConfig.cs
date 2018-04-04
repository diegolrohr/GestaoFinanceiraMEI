using System;
using System.Linq;
using RabbitMQ.Client;
using System.Web.Configuration;
using System.Collections.Generic;

namespace Fly01.Core.ServiceBus
{
    public static class RabbitConfig
    {
        public static string AppUser;
        public static string RoutingKey;
        public static string PlataformaUrl;
        public static string AppId => WebConfigurationManager.AppSettings["RabbitApplicationId"];
        public static string AMQPExchange => WebConfigurationManager.AppSettings["RabbitAMQPExchangeName"];
        public static string QueueName => WebConfigurationManager.AppSettings["RabbitServiceQueueName"];
        public static List<string> ListRoutingKeys => WebConfigurationManager.AppSettings["RabbitListRoutingKeys"].ToString().Split(',').ToList();

        public static ConnectionFactory Factory = new ConnectionFactory()
        {
            Uri = new Uri(WebConfigurationManager.AppSettings["RabbitAMQPUrl"]),
            VirtualHost = string.IsNullOrEmpty(WebConfigurationManager.AppSettings["RabbitVirtualHost"]) ? Environment.MachineName : WebConfigurationManager.AppSettings["RabbitVirtualHost"],
            UserName = string.IsNullOrEmpty(WebConfigurationManager.AppSettings["RabbitUserName"]) ? Environment.MachineName : WebConfigurationManager.AppSettings["RabbitUserName"],
            Password = string.IsNullOrEmpty(WebConfigurationManager.AppSettings["RabbitPassword"]) ? Environment.MachineName : WebConfigurationManager.AppSettings["RabbitPassword"]
        };

        public enum enHTTPVerb
        {
            POST,
            PUT,
            DELETE
        }
    }
}