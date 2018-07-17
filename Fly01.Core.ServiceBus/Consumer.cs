﻿using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fly01.Core.Notifications;
using System.Threading;
using Fly01.Core.Mensageria;

namespace Fly01.Core.ServiceBus
{
    public abstract class Consumer
    {
        private string MsgHeaderInvalid = "A 'PlataformaUrl', o 'Hostname' e o 'AppUser' devem ser informados no Header da request";

        private IConnection _connection;
        private IModel _channel;
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

        private IModel Channel
        {
            get
            {
                if (_channel == null)
                    _channel = Connection.CreateModel();

                return _channel;
            }
        }

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

        public void Consume()
        {
            var consumer = new AsyncEventingBasicConsumer(Channel);
            Channel.BasicConsume(RabbitConfig.QueueName, false, consumer);

            consumer.Received += async (sender, args) =>
            {
                try
                {
                    Channel.BasicAck(args.DeliveryTag, true);

                    if (args.BasicProperties.Headers == null)
                        throw new ArgumentException(MsgHeaderInvalid);

                    Headers = new Dictionary<string, object>(args.BasicProperties.Headers);
                    if (!HeaderIsValid())
                        throw new ArgumentException(MsgHeaderInvalid);

                    if (GetHeaderValue("Hostname") == RabbitConfig.VirtualHostname)
                    {
                        if (args.BasicProperties.AppId != RabbitConfig.AppId)
                        {
                            Message = Encoding.UTF8.GetString(args.Body);
                            HTTPMethod = (RabbitConfig.EnHttpVerb)Enum.Parse(typeof(RabbitConfig.EnHttpVerb), args.BasicProperties?.Type ?? "PUT");

                            RabbitConfig.PlataformaUrl = GetHeaderValue("PlataformaUrl");
                            RabbitConfig.AppUser = GetHeaderValue("AppUser");
                            RabbitConfig.RoutingKey = args.RoutingKey ?? string.Empty;

                            await PersistMessage();

                            foreach (var item in exceptions)
                            {
                                var erro = (item.Value is BusinessException) ? (BusinessException)item.Value : (Exception)item.Value;

                                SlackClient.PostErrorRabbitMQ(item.Key, erro, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, RabbitConfig.PlataformaUrl, RabbitConfig.RoutingKey);

                                continue;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SlackClient.PostErrorRabbitMQ("Erro RabbitMQ", ex, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, RabbitConfig.PlataformaUrl, RabbitConfig.RoutingKey);
                    Channel.BasicAck(args.DeliveryTag, true);
                }
            };
        }

        //public async Task ConsumeAsync()
        //{
        //    using (var channel = Connection.CreateModel())
        //    {
        //        var queueingConsumer = new QueueingBasicConsumer(channel);
        //        channel.BasicConsume(RabbitConfig.QueueName, false, "", queueingConsumer);

        //        while (true)
        //        {
        //            var args = queueingConsumer.Queue.Dequeue() as BasicDeliverEventArgs;

        //            try
        //            {
        //                channel.BasicAck(args.DeliveryTag, false);

        //                if (args.BasicProperties.Headers == null)
        //                    throw new ArgumentException(MsgHeaderInvalid);

        //                Headers = new Dictionary<string, object>(args.BasicProperties.Headers);
        //                if (!HeaderIsValid())
        //                    throw new ArgumentException(MsgHeaderInvalid);

        //                if (GetHeaderValue("Hostname") == RabbitConfig.VirtualHostname)
        //                {
        //                    if (args.BasicProperties.AppId != RabbitConfig.AppId)
        //                    {
        //                        Message = Encoding.UTF8.GetString(args.Body);
        //                        HTTPMethod = (RabbitConfig.EnHttpVerb)Enum.Parse(typeof(RabbitConfig.EnHttpVerb), args.BasicProperties?.Type ?? "PUT");

        //                        RabbitConfig.PlataformaUrl = GetHeaderValue("PlataformaUrl");
        //                        RabbitConfig.AppUser = GetHeaderValue("AppUser");
        //                        RabbitConfig.RoutingKey = args.RoutingKey ?? string.Empty;

        //                        await PersistMessage();

        //                        if (exceptions.Count > 0)
        //                        {
        //                            foreach (var item in exceptions)
        //                            {
        //                                var erro = (item.Value is BusinessException) ? (BusinessException)item.Value : (Exception)item.Value;

        //                                SlackClient.PostErrorRabbitMQ(item.Key, erro, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, RabbitConfig.PlataformaUrl, RabbitConfig.RoutingKey);
        //                                continue;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                SlackClient.PostErrorRabbitMQ("Erro RabbitMQ", ex, RabbitConfig.VirtualHostname, RabbitConfig.QueueName, RabbitConfig.PlataformaUrl, RabbitConfig.RoutingKey);
        //                continue;
        //            }
        //        }
        //    }
        //}
    }
}