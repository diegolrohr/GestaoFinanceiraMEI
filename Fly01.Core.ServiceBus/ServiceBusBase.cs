using System;
using Fly01.Core.Base;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Configuration;
using Fly01.Core.Entities.Domains.NoSQL;
using Fly01.Core.Helpers;

namespace Fly01.Core.ServiceBus
{
    public class ServiceBusBase : Consumer
    {
        private Type AssemblyBL { get; set; }

        public ServiceBusBase(Type assemblyBL)
        {
            SetupEnvironment.Create();
            AssemblyBL = assemblyBL;

            base.Consume();
        }

        protected override async Task PersistMessage()
        {
            var domainAssembly = Assembly.Load("Fly01.Core.Entities").GetType($"Fly01.Core.Entities.Domains.Commons.{RabbitConfig.RoutingKey}");
            var uow = AssemblyBL.GetConstructor(new Type[1] { typeof(ContextInitialize) }).Invoke(new object[] { new ContextInitialize() { AppUser = RabbitConfig.AppUser, PlataformaUrl = RabbitConfig.PlataformaUrl } });
            dynamic entidade = AssemblyBL.GetProperty(RabbitConfig.RoutingKey + "BL")?.GetGetMethod(false)?.Invoke(uow, null);
            exceptions = new List<KeyValuePair<string, object>>();

            foreach (var item in MessageType.Resolve<dynamic>(Message))
            {
                try
                {
                    var data = JsonConvert.DeserializeObject(item.ToString(), domainAssembly);

                    entidade.PersistMessage(data, HTTPMethod);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new KeyValuePair<string, object>(item.ToString(), ex));
                    continue;
                }
            }

            await (Task)AssemblyBL.GetMethod("Save").Invoke(uow, new object[] { });
        }

        protected override async Task PersistMessageIntegracao()
        {
            var logData = new LogIntegracaoRabbitMQ()
            {
                DataInclusao = DateTime.Now,
                HostName = RabbitConfig.VirtualHostname,
                Mensagem = Message,
                QueueName = RabbitConfig.QueueName,
                RoutingKey = RabbitConfig.RoutingKey.Replace("Integracao", ""),
                AppId = RabbitConfig.AppId
            };

            var mongoHelper = new LogMongoHelper<LogIntegracaoRabbitMQ>(ConfigurationManager.AppSettings["MongoDBLog"]);
            var collection = mongoHelper.GetCollection(ConfigurationManager.AppSettings["MongoCollectionNameIntegracaoRabbitMQ"]);

            await collection.InsertOneAsync(logData);
        }
    }
}