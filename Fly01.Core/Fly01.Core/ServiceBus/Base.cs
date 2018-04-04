using RabbitMQ.Client;
using System;
using System.Web.Configuration;

namespace Fly01.Utils.ServiceBus
{
    public class Base
    {
        protected static readonly ConnectionFactory connFactory = new ConnectionFactory();
        protected static string amqExchange;

        public Base()
        {
            ConfigureConn();
        }

        private void ConfigureConn()
        {
            var url = WebConfigurationManager.AppSettings["RabbitAMQPUrl"];

            connFactory.Uri = new Uri(url.Replace("amqp://", "amqps://"));
            amqExchange = "amq.direct";
        }
    }
}