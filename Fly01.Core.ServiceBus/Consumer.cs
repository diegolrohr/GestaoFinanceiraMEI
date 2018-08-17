using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fly01.Core.Notifications;
using Fly01.Core.Mensageria;
using Newtonsoft.Json;
using Fly01.Core.Base;

namespace Fly01.Core.ServiceBus
{
    public class Consumer
    {
        private readonly string MsgHeaderInvalid = "A 'PlataformaUrl', o 'Hostname' e o 'AppUser' devem ser informados no Header da mensagem";
        private readonly string MsgAppIdInvalid = "AppId não informado nas propriedades da mensagem";
        private readonly string MsgTypeInvalid = "Type (POST, PUT, DELETE) não informado nas propriedades da mensagem";
        private readonly string MsgRoutingKeyInvalid = "RoutingKey não informado no corpo da mensagem";
        private Type AssemblyBL;
        private Dictionary<string, object> Headers;

        public Consumer(Type assemblyBL)
        {
            if (AssemblyBL == null)
                AssemblyBL = assemblyBL;

            Headers = new Dictionary<string, object>();
        }

        private IModel GetChannel(string virtualHost)
        {
            var conn = new ConnectionFactory()
            {
                Uri = RabbitConfig.AMQPURL,
                UserName = RabbitConfig.UserName,
                Password = RabbitConfig.Password,
                VirtualHost = virtualHost
            };

            try
            {
                var channel = conn.CreateConnection($"cnsmr_{virtualHost}_{RabbitConfig.QueueName}").CreateModel();
                channel.BasicQos(0, 1, false);

                return channel;
            }
            catch (Exception ex)
            {
                MediaClient.PostErrorRabbitMQ($"Erro ao criar canal {ex.Message}", ex.InnerException, virtualHost, RabbitConfig.QueueName, GetHeaderValue("PlataformaUrl"), "");
                return null;
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
            var channelApps = GetChannel(RabbitConfig.VirtualHostApps);
            lock (channelApps)
                StartConsumer(channelApps);

            var channelIntegracao = GetChannel(RabbitConfig.VirtualHostIntegracao);
            lock (channelIntegracao)
                StartConsumer(channelIntegracao);
        }

        private void StartConsumer(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, args) =>
            {
                try
                {
                    Headers = new Dictionary<string, object>(args.BasicProperties.Headers ?? new Dictionary<string, object>());

                    if (args.BasicProperties.Headers == null || !HeaderIsValid()) throw new ArgumentException(MsgHeaderInvalid);
                    if (args.BasicProperties.AppId == null) throw new ArgumentException(MsgAppIdInvalid);
                    if (args.BasicProperties.Type == null) throw new ArgumentException(MsgTypeInvalid);
                    if (args.RoutingKey == null) throw new ArgumentException(MsgRoutingKeyInvalid);
                    if (args.BasicProperties.AppId == RabbitConfig.AppId) return;
                    if (GetHeaderValue("Hostname") != RabbitConfig.VirtualHostApps && GetHeaderValue("Hostname") != RabbitConfig.VirtualHostIntegracao) return;

                    var message = Encoding.UTF8.GetString(args.Body);
                    var httpMethod = (RabbitConfig.EnHttpVerb)Enum.Parse(typeof(RabbitConfig.EnHttpVerb), args.BasicProperties.Type);
                    var routingKey = args.RoutingKey;
                    var appId = args.BasicProperties.AppId;
                    var plataformaUrl = GetHeaderValue("PlataformaUrl");
                    var appUser = GetHeaderValue("AppUser");

                    await ProcessData(message, httpMethod, routingKey, appId, plataformaUrl, appUser);
                }
                catch (Exception ex)
                {
                    MediaClient.PostErrorRabbitMQ("Erro RabbitMQ", ex.InnerException, RabbitConfig.VirtualHostApps, RabbitConfig.QueueName, GetHeaderValue("PlataformaUrl"), args.RoutingKey);
                }
                finally
                {
                    channel.BasicAck(args.DeliveryTag, false);
                }
            };

            channel.BasicConsume(RabbitConfig.QueueName, false, consumer);
        }

        private async Task ProcessData(string message, RabbitConfig.EnHttpVerb httpMethod, string routingKey, string appId, string plataformaUrl, string appUser)
        {
            RpcClient rpc = new RpcClient();
            Object unitOfWork = AssemblyBL.GetConstructor(new Type[1] { typeof(ContextInitialize) }).Invoke(new object[] { new ContextInitialize() { AppUser = appUser, PlataformaUrl = plataformaUrl } });

            foreach (var item in MessageType.Resolve<dynamic>(message))
            {
                try
                {
                    dynamic entidade = AssemblyBL.GetProperty($"{routingKey}BL")?.GetGetMethod(false)?.Invoke(unitOfWork, null);
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(item.ToString());

                    entidade.PersistMessage(data, httpMethod, appId.ToLower() == "bemacash", rpc);

                    await (Task)AssemblyBL.GetMethod("Save").Invoke(unitOfWork, new object[] { });
                }
                catch (Exception exErr)
                {
                    var erro = (exErr is BusinessException) ? (BusinessException)exErr : exErr;
                    MediaClient.PostErrorRabbitMQ(item.ToString(), erro, RabbitConfig.VirtualHostApps, RabbitConfig.QueueName, plataformaUrl, routingKey);

                    continue;
                }
            }
            //rpc.Close();
        }
    }
}