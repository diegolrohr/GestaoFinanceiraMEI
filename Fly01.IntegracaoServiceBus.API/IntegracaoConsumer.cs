using Fly01.Core;
using Fly01.Core.Base;
using Fly01.Core.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Fly01.IntegracaoServiceBus.API
{
    public class IntegracaoConsumer : Consumer
    {
        private Type AssemblyBL { get; set; }

        protected override async Task PersistMessage()
        {
            AssemblyBL = Type.GetType($"{Assembly.GetExecutingAssembly().GetName().Name}.UnitOfWork");

            var domainAssembly = Assembly.Load("Fly01.Core.Entities").GetType($"Fly01.Core.Entities.Domains.Commons.{RabbitConfig.RoutingKey}");
            var uow = AssemblyBL.GetConstructor(new Type[1] { typeof(ContextInitialize) }).Invoke(new object[] { new ContextInitialize() { AppUser = RabbitConfig.AppUser, PlataformaUrl = RabbitConfig.PlataformaUrl } });
            dynamic entidade = AssemblyBL.GetProperty(RabbitConfig.RoutingKey + "BL")?.GetGetMethod(false)?.Invoke(uow, null);
            exceptions = new List<KeyValuePair<string, object>>();

            foreach (var item in MessageType.Resolve<dynamic>(Message))
            {
                try
                {
                    var data = JsonConvert.DeserializeObject(item.ToString(), domainAssembly);

                    //entidade.PersistMessage(data, HTTPMethod);

                    //await (Task)AssemblyBL.GetMethod("Save").Invoke(uow, new object[] { });
                }
                catch (Exception ex)
                {
                    exceptions.Add(new KeyValuePair<string, object>(item.ToString(), ex));
                    continue;
                }
            }

        }
    }
}