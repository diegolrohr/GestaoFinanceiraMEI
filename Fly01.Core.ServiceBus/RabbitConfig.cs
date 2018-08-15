using System;
using System.Linq;
using System.Web.Configuration;
using System.Collections.Generic;

namespace Fly01.Core.ServiceBus
{
    public static class RabbitConfig
    {
        public static Uri AMQPURL => new Uri(WebConfigurationManager.AppSettings["RabbitAMQPUrl"]);
        public static string UserName => WebConfigurationManager.AppSettings["RabbitUserName"];
        public static string Password => WebConfigurationManager.AppSettings["RabbitPassword"];
        public static string AppId => WebConfigurationManager.AppSettings["RabbitServiceQueueName"].ToLower();
        public static string AMQPExchange => WebConfigurationManager.AppSettings["RabbitAMQPExchangeName"];
        public static string QueueName => WebConfigurationManager.AppSettings["RabbitVirtualHostname"] == "dev" ? Environment.MachineName + "_" + WebConfigurationManager.AppSettings["RabbitServiceQueueName"] : WebConfigurationManager.AppSettings["RabbitServiceQueueName"];
        public static string VirtualHostApps => WebConfigurationManager.AppSettings["RabbitVirtualHostApps"];
        public static string VirtualHostIntegracao => WebConfigurationManager.AppSettings["RabbitVirtualHostIntegracao"];
        public static string VirtualHostname => WebConfigurationManager.AppSettings["RabbitVirtualHostname"] == "dev" ? Environment.MachineName : WebConfigurationManager.AppSettings["RabbitVirtualHostname"];
        public static List<string> ListRoutingKeys => WebConfigurationManager.AppSettings["RabbitListRoutingKeys"].ToString().Split(',').ToList();
        public static string SequenceGeneratorQueue => WebConfigurationManager.AppSettings["RabbitSequenceGenetorQueueName"];

        public enum EnHttpVerb
        {
            POST,
            PUT,
            DELETE
        }
    }
}