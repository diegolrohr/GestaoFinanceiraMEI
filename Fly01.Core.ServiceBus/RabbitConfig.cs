﻿using System;
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
        public static string QueueName => WebConfigurationManager.AppSettings["RabbitVirtualHostname"] == "dev"
            ? Environment.MachineName + "_" + WebConfigurationManager.AppSettings["RabbitServiceQueueName"]
            : WebConfigurationManager.AppSettings["RabbitServiceQueueName"];
        public static string VirtualHostname => WebConfigurationManager.AppSettings["RabbitVirtualHostname"] == "dev"
            ? Environment.MachineName
            : WebConfigurationManager.AppSettings["RabbitVirtualHostname"];
        public static List<string> ListRoutingKeys => WebConfigurationManager.AppSettings["RabbitListRoutingKeys"].ToString().Split(',').ToList();
        public static List<string> ListRoutingKeysIntegracao => WebConfigurationManager.AppSettings["RabbitListRoutingKeysIntegracao"].ToString().Split(',').ToList();
        public static string SequenceGeneratorQueue => WebConfigurationManager.AppSettings["RabbitSequenceGenetorQueueName"];

        public static ConnectionFactory Factory = new ConnectionFactory()
        {
            Uri = new Uri(WebConfigurationManager.AppSettings["RabbitAMQPUrl"]),
            UserName = WebConfigurationManager.AppSettings["RabbitUserName"],
            Password = WebConfigurationManager.AppSettings["RabbitPassword"],
        };

        public enum EnHttpVerb
        {
            POST,
            PUT,
            DELETE
        }
    }
}