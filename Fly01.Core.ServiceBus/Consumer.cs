using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using Fly01.Core.Mensageria;
using System.Collections.Generic;
using Fly01.Core.Notifications;
using System.Threading;

namespace Fly01.Core.ServiceBus
{
    public abstract class Consumer
    {
        private string MsgHeaderInvalid = "A 'PlataformaUrl', o 'Hostname' e o 'AppUser' devem ser informados no Header da request";

        private IConnection _connection;
        protected string Message;
        protected RabbitConfig.EnHttpVerb HTTPMethod;
        protected Dictionary<string, object> Headers = new Dictionary<string, object>();
        protected List<KeyValuePair<string, object>> exceptions = new List<KeyValuePair<string, object>>();
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

        //private IModel Channel
        //{
        //    get
        //    {
        //        var model = Connection.CreateModel();
        //        return model;
        //    }
        //}

        private bool HeaderIsValid()
        {
            return !string.IsNullOrWhiteSpace(GetHeaderValue("PlataformaUrl")) &&
                !string.IsNullOrWhiteSpace(GetHeaderValue("AppUser")) &&
                !string.IsNullOrWhiteSpace(GetHeaderValue("Hostname"));
        }

        private string GetHeaderValue(string key)
        {
            if (!Headers.ContainsKey(key))
                return string.Empty;

            return Encoding.UTF8.GetString(Headers[key] as byte[]);
        }

        public async void Consume()
        {
            using (var channel = Connection.CreateModel())
            {
                var queueingConsumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(RabbitConfig.QueueName, false, "", queueingConsumer);

                while (true)
                {
                    BasicDeliverEventArgs args = queueingConsumer.Queue.Dequeue() as BasicDeliverEventArgs;
                    channel.BasicAck(args.DeliveryTag, false);

                    if (args.BasicProperties.AppId != RabbitConfig.AppId)
                    {
                        if (args.BasicProperties.Headers == null)
                            throw new ArgumentException(MsgHeaderInvalid);

                        Headers = new Dictionary<string, object>(args.BasicProperties.Headers);
                        if (!HeaderIsValid())
                            throw new ArgumentException(MsgHeaderInvalid);

                        if (GetHeaderValue("Hostname") == RabbitConfig.VirtualHostname)
                        {
                            Message = Encoding.UTF8.GetString(args.Body);
                            HTTPMethod = (RabbitConfig.EnHttpVerb)Enum.Parse(typeof(RabbitConfig.EnHttpVerb), args.BasicProperties?.Type ?? "PUT");

                            RabbitConfig.PlataformaUrl = GetHeaderValue("PlataformaUrl");
                            RabbitConfig.AppUser = GetHeaderValue("AppUser");
                            RabbitConfig.RoutingKey = args.RoutingKey ?? string.Empty;

                            await PersistMessage();

                            if (exceptions.Count > 0)
                            {
                                foreach (var item in exceptions)
                                {
                                    var erro = (item.Value is BusinessException) ? (BusinessException)item.Value : (Exception)item.Value;

                                    //SlackClient.PostErrorRabbitMQ(item.Key, erro, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, RabbitConfig.PlataformaUrl, RabbitConfig.RoutingKey);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}